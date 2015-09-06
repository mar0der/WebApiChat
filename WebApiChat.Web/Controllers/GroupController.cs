using WebApiChat.Web.Models.Messages;

namespace WebApiChat.Web.Controllers
{
    #region

    using System;
    using System.Linq;
    using System.Web.Http;

    using WebApiChat.Models.Models;
    using WebApiChat.Web.Hubs;
    using WebApiChat.Web.Models;
    using WebApiChat.Web.Models.GroupChat;

    #endregion

    [Authorize]
    [RoutePrefix("api/group")]
    public class GroupController : ApiControllerWithHub<BaseHub>
    {
        [HttpGet]
        public IHttpActionResult GetAllGroupChats()
        {
            var groupChats =
                this.CurrentUser.GroupChats.Select(
                    g =>
                    new
                        {
                            GroupId = g.Id,
                            GroupName = g.Name ?? string.Join(", ", g.Users.Select(u => u.UserName))
                        });

            return this.Ok(groupChats);
        }

        [HttpPost]
        public IHttpActionResult CreateGroupChat(GroupChatBindingModel groupChatBindingModel)
        {
            var groupChat = new GroupChat { Name = groupChatBindingModel.GroupName };

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

            // TODO test 
            // TODO add view for the UI
            return this.Ok(new { groupChat.Id, users = groupChat.Users.Select(u => u.UserName) });
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
            return this.Ok(new { contact.ContactUser.Id, contact.ContactUser.UserName });
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

            return
                group.GroupMessages.Select(
                    gp =>
                    new GroupMessagesBindingModel
                        {
                            Id = gp.Id,
                            Content = gp.Text,
                            SenderId = gp.SenderId,
                            SenderUsername = gp.Sender.UserName,
                            Text = gp.Text
                        }).AsQueryable();
        }

        [HttpPost]
        [Route("addMessage")]
        public IHttpActionResult AddMessage(AddGroupMessageBindingModel groupMessage)
        {
            if (groupMessage == null)
            {
                return this.BadRequest("no data provided");
            }

            var group = this.CurrentUser.GroupChats.FirstOrDefault(g => g.Id == groupMessage.GroupId);

            if (group == null)
            {
                return this.BadRequest("Group was not found.");
            }

            var groupMessageForAdd = new GroupMessage
            {
              
                GroupChatId =  groupMessage.GroupId,
                Text = groupMessage.Text,
                SenderId = this.CurrentUserId,
                Date = DateTime.Now
            };

            var onlineUsers = ConnectionManager.Users.Keys;

            var groupOnlineUsers = group.Users.Where(g => onlineUsers.Contains(g.UserName) && g.UserName != this.CurrentUserUserName).ToList();

            var offlineGroupUsers = group.Users.Where(g => !onlineUsers.Contains(g.UserName) && g.UserName != this.CurrentUserUserName).ToList();

            groupMessageForAdd.RecievedUsers = groupOnlineUsers;


            //TODO check for optimisation. One is unnessary
            groupMessageForAdd.UnRecievedUsers = offlineGroupUsers;

            group.GroupMessages.Add(groupMessageForAdd);

            
      

            this.Data.SaveChanges();


            var messageViewModel = GroupMessageView.CreateOne(groupMessageForAdd);

            var uaasda = groupOnlineUsers.Select(u => u.UserName).ToList();

            this.HubContex.Clients.Users(uaasda).toggleGroupMessage(messageViewModel);



            return this.Ok(messageViewModel);
        }
    }
}