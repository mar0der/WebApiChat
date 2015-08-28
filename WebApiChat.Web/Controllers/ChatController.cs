using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using WebApiChat.Models.Models;
using WebApiChat.Web.Hubs;
using WebApiChat.Web.Models.Messages;
using WebGrease.Css.Extensions;

namespace WebApiChat.Web.Controllers
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiControllerWithHub<BaseHub>
    {

        [HttpGet]
        [System.Web.Http.Authorize]
        public IHttpActionResult GetChatWithContact(string id)
        {


            string currentUserId = this.User.Identity.GetUserId();

            var chats = this.Data.Users.All().Where(u => u.Id == currentUserId)
                 .Select(u => u.Chats
                     .Where(c => c.Users
                         .Any(chu => chu.Id == id)
                     ).Select(c => c.Id)).First();


            if (!chats.Any())
            {
                var chat = new Chat();
                chat.Users.Add(this.Data.Users.All().FirstOrDefault(u => u.Id == currentUserId));

                chat.Users.Add(this.Data.Users.All().FirstOrDefault(u => u.Id == id));

                this.Data.Chats.Add(chat);
                this.Data.SaveChanges();

                return this.Ok("new chat has been created");
            }




            var messages = this.Data.Users.All()
                .Where(u => u.Id == currentUserId)
                .Select(u => u.Chats
                    .Where(ch => ch.Users
                        .Any(uc => uc.Id == id))
                    .Select(ch => new
                    {
                        Messages = ch.Messages
                            .Select(m => new
                            {
                                Sender = m.Sender.UserName,
                                m.Text
                            }),
                        Name = ch.Id
                    })).FirstOrDefault();




            return this.Ok(messages);
        }


        [HttpPost]
        [System.Web.Http.Authorize]
        [Route("send/{chatId}")]
        public IHttpActionResult AddMessage(int chatId, MessageBindingModel messageBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (messageBindingModel == null)
            {
                return this.BadRequest("no data provided for message");
            }

            var message = new Message()
            {
                ChatId = chatId,
                SenderId = this.User.Identity.GetUserId(),
                Text = messageBindingModel.Text
            };


            var currentUserId = this.User.Identity.GetUserId();
            var currentUserUsername = this.User.Identity.GetUserName();

            var recieverUsernames = this.Data.Chats
                .All()
                .Where(c => c.Id == chatId)
                .SelectMany(x => x.Users
                    .Where(u => u.Id != currentUserId).Select(u => new
                    {
                        username = u.UserName
                    })
                ).Select(us => us.username).ToList();

            var onlineUsers = ConnectionManager.Users.Keys;
            var offlineUsers = this.Data.Chats
                .All()
                .Where(c => c.Id == chatId)
                .SelectMany(x => x.Users
                    .Where(u => !onlineUsers.Contains(u.UserName) && u.UserName != currentUserUsername).Select(u => new
                    {
                        username = u.UserName
                    })
                ).Select(us => us.username).ToList();
                    


            //logic for recieved
            //this.Data.Messages.All()
            //    .Where(m => m.IsRevieved == false).ForEach(m => m.IsRevieved = true);


            this.HubContex.Clients.Users(recieverUsernames).toggleMessage(new
            {
                Text = message.Text,
                Sender = messageBindingModel.SenderName
            });



            this.Data.Messages.Add(message);
            

            


            this.Data.SaveChanges();
            return this.Ok(new
            {
                Text = message.Text,
                Sender = messageBindingModel.SenderName
            });
        }
    }
}
