namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;

    using WebApiChat.Models.Enums;
    using WebApiChat.Models.Models;
    using WebApiChat.Web.Hubs;
    using WebApiChat.Web.Models.Messages;

    #endregion

    [RoutePrefix("api/chat")]
    [Authorize]
    public class ChatController : ApiControllerWithHub<BaseHub>
    {
        [HttpGet]
        [Route("{userId}")]
        public IHttpActionResult GetPrivateChatHistory(string userId)
        {
            var selectedUser = this.Data.Users.Find(userId);

            if (selectedUser == null)
            {
                return this.BadRequest("No such user!");
            }

            if (selectedUser.Id == this.CurrentUserId)
            {
                this.BadRequest("You cannot chat with yourself!");
            }

            this.CurrentUser.ReceivedMessages.Where(m => m.SenderId == userId)
                .Select(m => m.Status = MessageStatus.Seen)
                .ToList();

            this.Data.SaveChanges();

            var privateChatMessages =
                this.Data.Messages.All()
                    .Where(
                        u =>
                        u.SenderId == userId && u.ReceiverId == this.CurrentUserId
                        || u.ReceiverId == userId && u.SenderId == this.CurrentUserId)
                    .OrderByDescending(m => m.Id)
                    .Take(20)
                    .OrderBy(m => m.Id)
                    .Select(m => new { m.Id, m.Text, Sender = m.Sender.UserName, Status = m.Status.ToString() });

            return this.Ok(privateChatMessages);
        }

        [HttpGet]
        [Route("unreceived")]
        public IHttpActionResult GetUrecievedMessages()
        {
            var messages =
                this.CurrentUser.ReceivedMessages.Where(
                    m => m.Status != MessageStatus.Seen && m.ReceiverId == this.CurrentUserId)
                    .Select(
                        x =>
                        new
                            {
                                sender = x.Sender.UserName, 
                                count =
                            x.Sender.SentMessages.Where(
                                m => m.ReceiverId == this.CurrentUserId && m.Status != MessageStatus.Seen)
                            })
                    .GroupBy(x => new { x.sender })
                    .Select(x => new { x.Key.sender, count = x.Count() })
                    .ToList();

            return this.Ok(messages);
        }

        [HttpPost]
        [Route("unreceived/{id}")]
        public IHttpActionResult UpdateMessageStatus(int id)
        {
            var message = this.Data.Messages.Find(id);
            message.Status = MessageStatus.NotDelivered;
            this.Data.SaveChanges();
            return this.Ok("message updated");
        }

        [HttpPost]
        public IHttpActionResult PostMessageToChat(MessageBindingModel messageBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var receiver = this.Data.Users.Find(messageBindingModel.ReceiverId);
            if (receiver == null)
            {
                return this.BadRequest("The user you are trying to message does not exists!");
            }

            var currentUsers = ConnectionManager.Users.Keys;
            var message = new PrivateMessage
                              {
                                  Sender = this.CurrentUser, 
                                  SenderId = this.CurrentUserId, 
                                  Receiver = receiver, 
                                  ReceiverId = receiver.Id, 
                                  Text = messageBindingModel.Text
                              };

            if (currentUsers.Contains(receiver.UserName) && receiver.CurrentChatId == this.CurrentUser.Id)
            {
                message.Status = MessageStatus.Seen;
                // call signal r to the licent to display seend
            }
            else if (currentUsers.Contains(receiver.UserName))
            {
                message.Status = MessageStatus.Sent;
            }
            else
            {
                message.Status = MessageStatus.NotDelivered;
            }

            this.Data.Messages.Add(message);
            this.Data.SaveChanges();
            this.HubContex.Clients.User(receiver.UserName)
                .pushMessageToClient(
                    new
                        {
                            message.Id, 
                            message.Text, 
                            Sender = this.CurrentUserUserName, 
                            Receiver = receiver.UserName, 
                            Status = message.Status.ToString(), 
                            SenderId = this.CurrentUserId, 
                            ReceiverId = receiver.Id, 
                            MessageId = message.Id
                        });

            // TODO use viewModel
            return
                this.Ok(
                    new
                        {
                            message.Text, 
                            Sender = this.CurrentUserUserName, 
                            Status = message.Status.ToString(), 
                            MessageId = message.Id
                        });
        }

        [HttpPut]
        [Route("updateUserCurrentChatId/{chatId}")]
        public IHttpActionResult UpdateUserCurrentChatId(string chatId)
        {
            this.CurrentUser.CurrentChatId = chatId;
            this.Data.SaveChanges();
            var sender = this.Data.Users.Find(chatId);

            if (sender != null)
            {
                if (sender.CurrentChatId == this.CurrentUserId)
                {
                    var messages =
                        this.Data.Messages.All()
                            .Where(
                                m =>
                                m.SenderId == sender.Id && m.ReceiverId == this.CurrentUserId
                                || m.SenderId == this.CurrentUserId && m.ReceiverId == sender.Id)
                            .OrderByDescending(m => m.Id)
                            .Take(20)
                            .OrderBy(m => m.Id)
                            .Select(m => new { m.Id, Sender = m.Sender.UserName, m.Text, Status = m.Status.ToString() })
                            .ToList();

                    this.HubContex.Clients.User(sender.UserName).seenMessages(messages);
                }
            }

            return this.Ok();
        }
    }
}