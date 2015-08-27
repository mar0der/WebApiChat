using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace WebApiChat.Web.Controllers
{
    public class ChatController : BaseController
    {
        public IHttpActionResult GetChatWithContact(string id)
        {
            string currentUserId = this.User.Identity.GetUserId();

            var messages = this.Data.Users.All()
                .Where(u => u.Id == currentUserId)
                .Select(u => u.Chats
                    .Where(ch => ch.Users
                        .Any(uc => uc.Id == id))
                    .Select(ch => new
                    {
                        Messages = ch.Messages
                            .Select(m => new
                            {
                                Sender = m.Sender.UserName,
                                m.Text
                            }),
                        Name = ch.Id
                    })).FirstOrDefault();

            return this.Ok(messages);
        }

    }
}
