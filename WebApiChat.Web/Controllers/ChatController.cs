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

            var privateChatMessages =
                this.Data.Messages.All()
                    .OrderBy(m => m.Id)
                    .Take(20)
                    .Select(m => new { m.Text, Sender = m.Sender.UserName });

            return this.Ok(privateChatMessages);
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

            if (!currentUsers.Contains(receiver.UserName))
            {
                message.Status = MessageStatus.NotDelivered;
            }
            else
            {
                message.Status = MessageStatus.Sent;
            }

            this.HubContex.Clients.User(receiver.UserName)
                .toggleMessage(
                    new
                        {
                            message.Text, 
                            Sender = this.CurrentUserUserName, 
                            Receiver = receiver.UserName, 
                            Status = message.Status.ToString(), 
                            SenderId = this.CurrentUserId, 
                            ReceiverId = receiver.Id
                        });

            this.Data.Messages.Add(message);

            this.Data.SaveChanges();

            // TODO use viewModel
            return this.Ok(new { message.Text, Sender = this.CurrentUserUserName, Status = message.Status.ToString() });
        }
    }
}