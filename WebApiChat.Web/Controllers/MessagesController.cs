using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using WebApiChat.Web.Hubs;

namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using WebApiChat.Data;
    using WebApiChat.Data.Interfaces;
    using WebApiChat.Data.Repositories;
    using WebApiChat.Models.Models;
    using WebApiChat.Web.Models.Messages;

    #endregion

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/messages")]
    public class MessagesController : BaseController
    {

        //[Route("id")]
        //public IHttpActionResult GetMessageFromUser(string id)
        //{
        //    var currentUserId = this.User.Identity.GetUserId();

        //    var messages = this.Data.Messages.All()
        //        .Where(m=>m.)
        //}

        [HttpPost]
        [Authorize]
        [Route("{chatId}")]

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

            //create new chat if nessesary
            //maybe add in binding model userId 

            var message = new Message()
            {
                ChatId = chatId,
                SenderId = this.User.Identity.GetUserId(),
                Text = messageBindingModel.Text
            };

         
            //var users = this.Data.Chats
            //    .All()
            //    .Where(c => c.Id == chatId)
            //    .SelectMany(cu => cu.Users.Select(u => u.Id))
            //    .ToList();

            var currentUserId = this.User.Identity.GetUserId();

            var users = this.Data.Chats.All()
                .Where(c => c.Id == chatId)
                .SelectMany(c => c.Users
                    .Where(u => u.Id != currentUserId))
                .Select(us => us.Id).ToList();

            var a = new
            {
                Text = message.Text,
                Sender = messageBindingModel.SenderName
            };

            this.Data.Messages.Add(message);
            this.Data.SaveChanges();

           // var _context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            // context.Clients.Users(users).sendToUsers(message);
         //  _context.Clients.All.sendToUsers(a);
            
            //_context.Clients.User("c52b2a96-8258-4cb0-b844-a6e443acb04b").sendToUsers(a);
            


            ///_context.Clients.Users(users).sendToUsers(message);
            return this.Ok();
        }


        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            var messages = this.Data.Messages.All().Select(MessageViewModel.ViewModel);

            return this.Ok(messages);
        }
    }
}