namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;

    using WebApiChat.Models.Models;
    using WebApiChat.Web.Hubs;
    using WebApiChat.Web.Models;

    #endregion

    [RoutePrefix("api/contacts")]
    [Authorize]
    public class ContactsController : ApiControllerWithHub<BaseHub>
    {
        /// <summary>
        ///     Returns all contacts with their status. Uses Current user
        /// </summary>
        /// <returns>Array of objects</returns>
        public IHttpActionResult GetContacts()
        {
            var currentUserId = this.User.Identity.GetUserId();

            var contacts = this.Data.Contacts.All().Where(c => c.UserId == currentUserId);

            var onlineUsers = ConnectionManager.Users.Keys;

            return this.Ok(
                contacts.Select(
                    c => new
                             {
                                 // ContactsOwer = c.ContactUser.UserName,
                                 Id = c.ContactUserId,
                                 c.ContactUser.UserName,
                                 c.ContactUser.LastName,
                                 c.ContactUser.FirstName,
                                 IsOnline = onlineUsers.Contains(c.ContactUser.UserName),

                                 // TODO: Why messages are 0 
                                 UnreceivedMessages = 0
                             }).ToList());
        }

        /// <summary>
        ///     Add contant to user
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
            var contact = currentUser.Contacts.FirstOrDefault(c => c.ContactUserId == id);

            if (contact != null)
            {
                return this.BadRequest("Already friends.");
            }

            userContact.Contacts.Add(new Contact { User = userContact, ContactUser = currentUser });

            currentUser.Contacts.Add(new Contact { User = currentUser, ContactUser = userContact });

            this.Data.SaveChanges();
            var onlineUsers = ConnectionManager.Users.Keys;
            var addedContactUser =
                new
                    {
                        userContact.Id,
                        userContact.UserName,
                        IsOnline = onlineUsers.Contains(userContact.UserName),
                        UnreceivedMessages = 0
                    };

            this.HubContex.Clients.User(userContact.UserName).contactListUpdate();

            return this.Ok(addedContactUser);
        }

        /// <summary>
        ///     Block contact in current user`s contacts by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>message</returns>
        [HttpPost]
        [Route("block/{id}")]
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
            this.Data.SaveChanges();

            return this.Ok("contact blocked");
        }

        [HttpGet]
        [Route("searchUser")]
        public IHttpActionResult SearchUser([FromUri] string searchPattern)
        {
            var contacts =
                this.Data.Contacts.All().Where(c => c.UserId == this.CurrentUserId).Select(u => u.ContactUser.UserName);

            var users =
                this.Data.Users.All()
                    .Where(
                        u =>
                        (u.FirstName.Contains(searchPattern) || u.Email.Contains(searchPattern)
                         || u.PhoneNumber.Contains(searchPattern) || u.LastName.Contains(searchPattern))
                        && u.UserName != this.CurrentUserUserName && !contacts.Contains(u.UserName))
                    .Select(UserSearchViewModel.ViewModel)
                    .Take(5)
                    .ToList();

            return this.Ok(users);
        }

        [HttpGet]
        [Route("searchInContacts")]
        public IHttpActionResult SearchInContacts([FromUri] string searchPattern)
        {
            var users =
                this.Data.Contacts.All()
                .Where(c => c.UserId == this.CurrentUserId &&
                    (c.ContactUser.FirstName.Contains(searchPattern)
                    || c.ContactUser.Email.Contains(searchPattern)
                    || c.ContactUser.PhoneNumber.Contains(searchPattern) 
                    || c.ContactUser.LastName.Contains(searchPattern)))
                    .Select(UserSearchViewModel.ViewModelFromContact)
                    .Take(5)
                    .ToList();

            return this.Ok(users);
        }
    }
}