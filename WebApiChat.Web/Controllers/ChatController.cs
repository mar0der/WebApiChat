﻿namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using WebApiChat.Models.Models;
    using WebApiChat.Web.Hubs;

    #endregion

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/chat")]
    public class ChatController : ApiControllerWithHub<BaseHub>
    {
        //[HttpGet]
        //[Authorize]
        //public IHttpActionResult GetChatWithContact(string id)
        //{
        //    var currentUserId = this.User.Identity.GetUserId();

        //    var chats =
        //        this.Data.Users.All()
        //            .Where(u => u.Id == currentUserId)
        //            .Select(u => u.Chats.Where(c => c.Users.Any(chu => chu.Id == id)).Select(c => c.Id))
        //            .First();

        //    if (!chats.Any())
        //    {
        //        var chat = new Chat();
        //        chat.Users.Add(this.Data.Users.All().FirstOrDefault(u => u.Id == currentUserId));

        //        chat.Users.Add(this.Data.Users.All().FirstOrDefault(u => u.Id == id));

        //        this.Data.Chats.Add(chat);
        //        this.Data.SaveChanges();

        //        return this.Ok("new chat has been created");
        //    }

        //    var messages =
        //        this.Data.Users.All()
        //            .Where(u => u.Id == currentUserId)
        //            .Select(
        //                u =>
        //                u.Chats.Where(ch => ch.Users.Any(uc => uc.Id == id))
        //                    .Select(
        //                        ch =>
        //                        new
        //                            {
        //                                Messages = ch.Messages.Select(m => new { Sender = m.Sender.UserName, m.Text }),
        //                                Name = ch.Id
        //                            }))
        //            .FirstOrDefault();

        //    return this.Ok(messages);
        //}

        //[HttpPost]
        //[Authorize]
        //[Route("send/{chatId}")]
        //public IHttpActionResult AddMessage(int chatId, MessageBindingModel messageBindingModel)
        //{
        //    var currentUserId = this.User.Identity.GetUserId();
        //    var currentUserUsername = this.User.Identity.GetUserName();

        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.BadRequest(this.ModelState);
        //    }

        //    if (messageBindingModel == null)
        //    {
        //        return this.BadRequest("no data provided for message");
        //    }

        //    var chat = this.Data.Chats.Find(chatId);

        //    if (chat == null)
        //    {
        //        return this.BadRequest("Invalid chat id");
        //    }

        //    var message = new Message
        //    {
        //        ChatId = chatId,
        //        SenderId = this.User.Identity.GetUserId(),
        //        Text = messageBindingModel.Text
        //    };

        //    this.Data.Messages.Add(message);
        //    this.Data.SaveChanges();

        //    var messageReceivers =
        //        chat.Users
        //        .Where(u => u.Id != currentUserId)
        //            .Select(u => new MessageReceiver()
        //            {
        //                MessageId = message.Id,
        //                ReceiverId = u.Id,
        //                Status = MessageStatusEnum.Sent
        //            })
        //            .ToList();

        //    message.Receivers = messageReceivers;

        //    this.Data.SaveChanges();

        //    var messageReceiversUsername = messageReceivers.Select(m => m.Receiver.UserName).ToList();
        //        //this.Data.Chats.All()
        //        //    .Where(c => c.Id == chatId)
        //        //    .SelectMany(
        //        //        x => x.Users.Where(u => u.Id != currentUserId).Select(u => new { username = u.UserName }))
        //        //    .Select(us => us.username)
        //        //    .ToList();

        //    var onlineUsers = ConnectionManager.Users.Keys;
        //    var offlineUsers = messageReceivers
        //        .Where(m => !onlineUsers.Contains(m.Receiver.UserName))
        //        .Select(m => m.Receiver.UserName)
        //        .ToList();
        //        //this.Data.Chats.All()
        //        //    .Where(c => c.Id == chatId)
        //        //    .SelectMany(
        //        //        x =>
        //        //        x.Users.Where(u => !onlineUsers.Contains(u.UserName) && u.UserName != currentUserUsername)
        //        //            .Select(u => new { username = u.UserName }))
        //        //    .Select(us => us.username)
        //        //    .ToList();

        //    // logic for recieved
        //    // this.Data.Messages.All()
        //    // .Where(m => m.IsRevieved == false).ForEach(m => m.IsRevieved = true);
        //    this.HubContex.Clients.Users(messageReceiversUsername)
        //        .toggleMessage(new { message.Text, Sender = messageBindingModel.SenderName });

        //    this.Data.SaveChanges();
        //    return this.Ok(new { message.Text, Sender = messageBindingModel.SenderName });
        //}


        [HttpPost]
        [Authorize]
        [Route("open")]
        public IHttpActionResult OpenChat(string userId)
        {
            var currentUser = this.Data.Users.Find(this.CurrentUserId);

            var chatUser = this.Data.Users.Find(userId);

            if (chatUser == null)
            {
                return this.BadRequest("invalid userId");
            }

            var exsitingChatId = this.Data.Chats
                .All()
                .Where(c => c.Users
                    .Contains(currentUser) &&
                            c.Users.Contains(chatUser) &&
                            c.Users.Count == 2)
                            .Select(c => c.Id)
                            .FirstOrDefault();

            // TODO Test if ==0 is correct
            if (exsitingChatId == 0)
            {
                var chat = new Chat();
                chat.Users.Add(chatUser);
                chat.Users.Add(currentUser);
                this.Data.Chats.Add(chat);
                this.Data.SaveChanges();
                return this.Ok(chat.Id);
            }

            return this.Ok(exsitingChatId);
        }
    }

}