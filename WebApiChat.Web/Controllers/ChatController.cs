﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web.Http;
using WebApiChat.Models.Enums;
using WebApiChat.Models.Models;
using WebApiChat.Web.Hubs;
using WebApiChat.Web.Models.Messages;

namespace WebApiChat.Web.Controllers
{
    [RoutePrefix("api/chat")]
    [Authorize]
    public class ChatController : ApiControllerWithHub<BaseHub>
    {
        [HttpGet]
        [Route("{userId}")]
        public IHttpActionResult GetCurrentChat(string userId)
        {

            var selectedUser =
                this.Data.Contacts.All().FirstOrDefault(c => c.UserId == this.CurrentUserId && c.ContactUser.Id == userId);

            if (selectedUser == null)
            {
                return this.BadRequest("no such user in contacts");
            }

            var chat = this.Data.Users.All()
                .Where(u => u.Id == CurrentUserId)
                .Select(u => u.Chats.FirstOrDefault(c => c.Users.Any(chu => chu.Id == userId))).FirstOrDefault();



            if (chat == null)
            {
                var createdChat = new PrivateChat();
                createdChat.Users.Add(this.Data.Users.All().FirstOrDefault(u => u.Id == this.CurrentUserId));
                createdChat.Users.Add(this.Data.Users.All().FirstOrDefault(u => u.Id == userId));
                this.Data.Chats.Add(createdChat);
                this.Data.SaveChanges();
                return this.Ok(new
                {
                    Id = createdChat.Id,
                    Messages = createdChat.Messages.Select(m => new
                    {
                        Text = m.Text,
                        Sender = m.Sender.UserName
                    })
                });
            }



            //TODO finish this

            return this.Ok(new
            {
                Id = chat.Id,
                Messages = chat.Messages.Select(m => new
                {
                    Text = m.Text,
                    Sender = m.Sender.UserName
                })
            });

        }

        [HttpPost]
        [Route("{id}")]
        public IHttpActionResult PostMessageToChat(int id, MessageBindingModel messageBindingModel)
        {
            var chat = this.Data.Chats.Find(id);

            if (chat == null)
            {
                return this.BadRequest("no such chat");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUsers = ConnectionManager.Users.Keys;

            var senderName = this.CurrentUserUserName;

            var reciever = chat.Users
                .Where(u => u.UserName != senderName)
                .Select(u => u.UserName)
                .FirstOrDefault();

            var message = new PrivateMessage()
            {
                ChatId = id,
                SenderId = this.CurrentUserId,
                Text = messageBindingModel.Text
            };

            if (!currentUsers.Contains(reciever))
            {
                message.Status = MessageStatus.NotDelivered;
            }

            else
            {
                message.Status = MessageStatus.Sent;


            }

            chat.Messages.Add(message);

            this.HubContex.Clients.User(reciever).toggleMessage(new
            {
                Text = message.Text,
                Sender = this.CurrentUserUserName,
                Status = message.Status.ToString(),
                SenderId = this.CurrentUserId,
                CurrentChatId = chat.Id
            });

            this.Data.SaveChanges();

            // TODO fix the return model for the UI
            return this.Ok(new
            {
                Text = message.Text,
                Sender = this.CurrentUserUserName,
                Status = message.Status.ToString()
            });
        }


        //[HttpGet]
        //[Route("unreceived")]
        //public IHttpActionResult GetUnrecievedMessages()
        //{
        //    //var messages = this.Data.Users.All()
        //    //    .Where(u => u.Id == this.CurrentUserId)
        //    //    .Select(
        //    //        x => new {

        //    //           users = x.UserName,
        //    //           count = x.Messages.Where(m => m.SenderId != this.CurrentUserId && m.Status == MessageStatus.NotDelivered)
        //    //                .GroupBy(m => m.SenderId).Count()
        //    //                }

        //    //    ).ToList();


        //    var messages = this.CurrentUser.Contacts
        //        .Select(c => new
        //        {
        //            user = c.ContactUser.UserName,
        //            count = c.ContactUser.Messages.Where(m => m.)
        //        })

        //    //{gosho : 3 , pesho : 2}


        //    return this.Ok(messages);
        //}

       
    }
}
