using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiChat.Models.Enums;
using WebApiChat.Models.Models;
using WebApiChat.Web.Hubs;
using WebApiChat.Web.Models;
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
            var groupChats = this.CurrentUser.GroupChats
                .Select(g => new
                {
                    GroupId = g.Id,
                    GroupName = g.Name.Length == 0 ? string.Join(", ", g.Users.Select(u => u.UserName)) : g.Name
                });


            return this.Ok(groupChats);
        }

        [HttpPost]
        public IHttpActionResult CreateGroupChat(GroupChatBindingModel groupChatBindingModel)
        {
            var groupChat = new GroupChat()
            {
                Name = groupChatBindingModel.GroupName
            };

            groupChat.Users.Add(this.CurrentUser);

            if (groupChatBindingModel.UserIds.Length == 0)
            {
                return this.BadRequest("Cannot create group without contacts.");
            }

            foreach (var userId in groupChatBindingModel.UserIds)
            {
                var currentContactUser = this.Data.Users.Find(userId);
                if (currentContactUser == null)
                {
                    return this.BadRequest("Found invalid user id.");
                }

                groupChat.Users.Add(currentContactUser);
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

        [HttpPost]
        [Route("{chatId}/{userId}")]
        public IHttpActionResult AddUserToGroupChat(int chatId, string userId)
        {
            var groupChat = this.Data.GroupChats.Find(chatId);

            if (groupChat == null)
            {
                return this.BadRequest("no such chat");
            }

            var contact = this.CurrentUser.Contacts.FirstOrDefault(c => c.ContactUserId == userId);

            if (contact == null)
            {
                return this.BadRequest("no such user in the contacts");
            }

            groupChat.Users.Add(contact.ContactUser);
            this.Data.SaveChanges();
            // TODO test 

            return this.Ok(new
            {
                contact.ContactUser.Id,
                contact.ContactUser.UserName
            });
        }

        [HttpGet]
        [Route("getMessages/{groupId}")]
        public IQueryable<GroupMessagesBindingModel> GetGroupMessages(int groupId)
        {
            var group = this.CurrentUser.GroupChats.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                throw new ApplicationException("Group not found.");
            }

            return group.GroupMessages
                    .Select(gp => new GroupMessagesBindingModel()
                    {
                        Id = gp.Id,
                        Content = gp.Text,
                        SenderId = gp.SenderId,
                        SenderUsername = gp.Sender.UserName
                    })
                    .AsQueryable<GroupMessagesBindingModel>();
        }

        [HttpPost]
        [Route("addMessage")]
        public IHttpActionResult AddMessage(AddGroupMessageBindingModel groupMessage)
        {
            var group = this.CurrentUser.GroupChats.FirstOrDefault(g => g.Id == groupMessage.GroupId);

            if(group == null)
            {
                return this.BadRequest("Group was not found.");
            }

            group.GroupMessages.Add(new GroupMessage()
            {
                Text = groupMessage.Text,
                SenderId = this.CurrentUserId,
                Date = DateTime.Now
            });

            this.Data.SaveChanges();

            return this.Ok("Message was added.");
        }
    }
}
