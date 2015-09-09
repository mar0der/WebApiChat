namespace WebApiChat.Web.Controllers
{
    #region

    using System;
    using System.Linq;
    using System.Web.Http;

    using WebApiChat.Models.Enums;
    using WebApiChat.Models.Models;
    using WebApiChat.Web.Hubs;
    using WebApiChat.Web.Models.GroupChat;
    using WebApiChat.Web.Models.GroupMessage;

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
                            GroupName =
                        g.Name.Trim().Length == 0 ? string.Join(", ", g.Users.Select(u => u.UserName)) : g.Name
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

            // need to be query
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

            var result = new { GroupId = groupChat.Id, GroupName = groupChat.Name };

            var groupUsers =
                groupChat.Users.Where(u => u.UserName != this.CurrentUserUserName).Select(u => u.UserName).ToList();
            this.HubContex.Clients.Users(groupUsers).toggleGroupCreation(result);

            // TODO test 
            // TODO add view for the UI
            return this.Ok(result);
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
        public IQueryable<GroupMessageViewModel> GetGroupMessages(int groupId)
        {
            var group = this.CurrentUser.GroupChats.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                throw new ApplicationException("Group not found.");
            }

            var groupMessages = group.GroupMessages
                .OrderByDescending(m => m.Id)
                .Take(20)
                .OrderBy(m => m.Id)
                .ToList();

            var groupMessagesView = groupMessages.Select(
                    gm =>
                    new GroupMessageViewModel()
                        {
                            Id = gm.Id,
                            GroupId = gm.GroupChatId,
                            Sender = gm.Sender.UserName,
                            Text = gm.Text
                        });

            return groupMessagesView.AsQueryable();
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


            var onlineUsers = ConnectionManager.Users.Keys;
            var groupOnlineUsers =
                group.Users.Where(g => onlineUsers.Contains(g.UserName) && g.UserName != this.CurrentUserUserName)
                    .ToList();

            var groupReceivers =
               this.Data.GroupChats.Find(groupMessage.GroupId).Users
               .Where(u => u.Id != this.CurrentUserId)
               .Select(u => new GroupMessageReceiver()
               {
                   Receiver = u,
                   Status = MessageStatus.NotDelivered
               })
               .ToList();

            foreach (var receiver in groupReceivers)
            {
                if (groupOnlineUsers.Contains(receiver.Receiver)
                    && receiver.Receiver.CurrentChatId == groupMessage.GroupId.ToString())
                {
                    receiver.Status = MessageStatus.Seen;
                }
                else if (groupOnlineUsers.Contains(receiver.Receiver))
                {
                    receiver.Status = MessageStatus.Sent;
                }
            }

            var groupMessageForAdd = new GroupMessage
                                         {
                                             GroupChatId = groupMessage.GroupId,
                                             Text = groupMessage.Text,
                                             SenderId = this.CurrentUserId,
                                             Date = DateTime.Now,
                                             GroupMessageReceivers = groupReceivers
                                         };

            group.GroupMessages.Add(groupMessageForAdd);
            this.Data.SaveChanges();

            var messageViewModel = GroupMessageViewModel.CreateOne(groupMessageForAdd);

            var onlineUserNamesList = groupOnlineUsers.Select(u => u.UserName).ToList();

            this.HubContex.Clients.Users(onlineUserNamesList).pushGroupMessage(messageViewModel);

            return this.Ok(messageViewModel);
        }

        [HttpGet]
        [Route("unreceived")]
        public IHttpActionResult GetMissedMessages()
        {
            var messages = this.Data.GroupMessages.All()
                .GroupBy(gm => new
                {
                    Name = gm.GroupChat.Name,
                    Id = gm.GroupChat.Id
                })
                .Select(gc => new
                {
                    Name = gc.Key.Name,
                    Count = gc.Count(m => m.GroupMessageReceivers
                        .Any(r => r.ReceiverId == this.CurrentUserId && r.Status != MessageStatus.Seen))
                });

            return this.Ok(messages);
        }
    }
}