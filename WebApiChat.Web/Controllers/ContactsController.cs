using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using WebApiChat.Models.Models;
using WebApiChat.Web.Hubs;

namespace WebApiChat.Web.Controllers
{
    [RoutePrefix("api/contacts")]
    [System.Web.Http.Authorize]
    public class ContactsController : ApiControllerWithHub<BaseHub>
    {
        /// <summary>
        /// Returns all contacts with their status. Uses Current user
        /// </summary>
        /// <returns>Array of objects</returns>
        public IHttpActionResult GetContacts()
        {
            var currentUserId = this.User.Identity.GetUserId();

            var contacts = this.Data.Contacts.All().Where(c => c.UserId == currentUserId);

            var onlineUsers = ConnectionManager.Users.Keys;

            return this.Ok(contacts.Select(c => new
            {
                ContactsOwer = c.ContactUser.UserName,
                User = c.ContactUser.UserName,
                ContactUserId = c.ContactUserId,
                IsOnline = onlineUsers.Contains(c.ContactUser.UserName)
            }).ToList());
        }

        /// <summary>
        /// Add contant to user
        /// </summary>
        /// <param name="id">string userId</param>
        /// <returns>Contact object with Id and UserName</returns>
        [HttpPost]
        public IHttpActionResult AddContact(string id)
        {
            var userContact = this.Data.Users.Find(id);

            if (userContact == null)
            {
                return this.BadRequest("No such user");
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            userContact.Contacts.Add(new Contact()
            {
                User =  userContact,
                ContactUser = currentUser
            });

            currentUser.Contacts.Add(new Contact()
            {
                User = currentUser,
                ContactUser = userContact
            });

            this.Data.SaveChanges();

            return this.Ok(new
            {
                Id = userContact.Id,
                UserName = userContact.UserName
            });
        }

        /// <summary>
        /// Block contact in current user`s contacts by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>message</returns>
        [HttpPost]
        [Route("block")]
        public IHttpActionResult BlockContact(string id)
        {
            var user = this.Data.Users.Find(id);

            if (user == null)
            {
                return this.BadRequest("no such user");
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.Contacts.All(u => u.ContactUser != user))
            {
                return this.BadRequest("No such contact in your contacts");
            }

            currentUser.Contacts.First(c => c.ContactUser == user).IsBlocked = true;

            return this.Ok("contact blocked");
        }
    }
}