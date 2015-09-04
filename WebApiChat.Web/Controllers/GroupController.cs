using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiChat.Models.Models;
using WebApiChat.Web.Hubs;
using WebApiChat.Web.Models.GroupChat;

namespace WebApiChat.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/group")]
    public class GroupController : ApiControllerWithHub<BaseHub>
    {
        [HttpGet]
        public IHttpActionResult GetAllGroupChats()
        {
            var groupChats = this.Data.GroupChats
                .All()
                .Where(g => g.Users
                    .Any(u => u.Id == this.CurrentUserId)
                ).ToList();

            return this.Ok(groupChats);
        }

        [HttpPost]
        public IHttpActionResult CreateGroupChat(GroupChatBindingModel groupChatBindingModel)
        {
            var groupChat = new GroupChat()
            {
                Name = groupChatBindingModel.GroupName
            };

            foreach (var userId in groupChatBindingModel.UserIds)
            {
                groupChat.Users.Add(this.Data.Users.Find(userId));
            }

            this.Data.GroupChats.Add(groupChat);
            this.Data.SaveChanges();

            //TODO test 
            //TODO add view for the UI
            return this.Ok(new
            {
                groupChat.Id,
                users = groupChat.Users.Select(u => u.UserName)
            });
        }

 //       [HttpPost]
 //       [Route("{chatId}/{userId}")]
 //       public IHttpActionResult AddUserToGroupChat(int chatId, string userId)
 //       {
 //           var groupChat = this.Data.Chats.Find(chatId);

 //           if (groupChat == null)
 //           {
 //               return this.BadRequest("no such chat");
 //           }

 //           var userToAdd = this.Data.Contacts
 //               .All()
 //               .Where(c => c.UserId == this.CurrentUserId && c.ContactUserId == userId)
 //               .Select(x => x.User)
 //.              FirstOrDefault();


 //           if (userToAdd == null)
 //           {
 //               return this.BadRequest("no such user in the contacts");
 //           }

 //           groupChat.Users.Add(userToAdd);

 //           // TODO test 

 //           return this.Ok(new
 //           {
 //               userToAdd.Id,
 //               userToAdd.UserName
 //           });
 //       }
    }
}
