namespace WebApiChat.Models.Models
{
    #region

    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    #endregion

    public class User : IdentityUser
    {
        private ICollection<Chat> chats;

        private ICollection<Contact> contacts;

        private ICollection<Message> messages;

        public User()
        {
            this.messages = new HashSet<Message>();
            this.chats = new HashSet<Chat>();
            this.contacts = new HashSet<Contact>();
        }


        public int Age { get; set; }

        // [Required]
        public string FirstName { get; set; }

        // [Required]
        public string LastName { get; set; }

        public virtual ICollection<Message> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
            }
        }

        public virtual ICollection<Chat> Chats
        {
            get
            {
                return this.chats;
            }

            set
            {
                this.chats = value;
            }
        }

        public virtual ICollection<Contact> Contacts
        {
            get
            {
                return this.contacts;
            }

            set
            {
                this.contacts = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<User> manager, 
            string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            return userIdentity;
        }
    }
}