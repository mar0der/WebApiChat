﻿using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using System.Web.OData.Routing;
using Microsoft.Ajax.Utilities;

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
                .Select(m => m.Status = MessageStatus.Sent).ToList();

            this.Data.SaveChanges();

            var privateChatMessages = this.Data.Messages.All()
                   .Where(u => u.SenderId == userId && u.ReceiverId == this.CurrentUserId
                       || u.ReceiverId == userId && u.SenderId == this.CurrentUserId)
                   .OrderBy(m => m.Id)
                   .Take(20)
                   .Select(m => new { m.Text, Sender = m.Sender.UserName });

            return this.Ok(privateChatMessages);
        }


        [HttpGet]
        [Route("unreceived")]
        public IHttpActionResult GetUrecievedMessages()
        {
            var messages = this.CurrentUser.ReceivedMessages
                .Where(m => m.Status == MessageStatus.NotDelivered && m.ReceiverId == this.CurrentUserId)
                .Select(x => new
                {
                    sender = x.Sender.UserName,
                    count =
                        x.Sender.SentMessages.Where(
                            m => m.ReceiverId == this.CurrentUserId && m.Status == MessageStatus.NotDelivered)
                })
                .GroupBy(x => new { sender = x.sender })
                .Select(x => new
                {
                    sender = x.Key.sender,
                    count = x.Count()
                })
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

            message.Status = !currentUsers.Contains(receiver.UserName) ? MessageStatus.NotDelivered : MessageStatus.Sent;

            this.Data.Messages.Add(message);
            this.Data.SaveChanges();


            this.HubContex.Clients.User(receiver.UserName)
                .toggleMessage(
                    new
                        {
                            message.Text,
                            Sender = this.CurrentUserUserName,
                            Receiver = receiver.UserName,
                            Status = message.Status.ToString(),
                            SenderId = this.CurrentUserId,
                            ReceiverId = receiver.Id,
                            MessageId = message.Id
                        });


            // TODO use viewModel
            return this.Ok(new { message.Text, Sender = this.CurrentUserUserName, Status = message.Status.ToString(), MessageId = message.Id });
        }
    }
}