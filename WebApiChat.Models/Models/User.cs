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
        private ICollection<Contact> contacts;

        private ICollection<GroupChat> groupChats;

        private ICollection<GroupMessage> groupMessages;

        private ICollection<PrivateMessage> privateReceivedMessages;

        private ICollection<PrivateMessage> privateSentMessages;

        public User()
        {
            this.privateSentMessages = new HashSet<PrivateMessage>();
            this.privateReceivedMessages = new HashSet<PrivateMessage>();
            this.contacts = new HashSet<Contact>();
            this.groupChats = new HashSet<GroupChat>();
            this.groupMessages = new HashSet<GroupMessage>();
        }

        public int Age { get; set; }

        // [Required]
        public string FirstName { get; set; }

        public string CurrentChatId { get; set; }

        // [Required]
        public string LastName { get; set; }

        public virtual ICollection<PrivateMessage> SentMessages
        {
            get
            {
                return this.privateSentMessages;
            }

            set
            {
                this.privateSentMessages = value;
            }
        }

        public virtual ICollection<PrivateMessage> ReceivedMessages
        {
            get
            {
                return this.privateReceivedMessages;
            }

            set
            {
                this.privateReceivedMessages = value;
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

        public virtual ICollection<GroupChat> GroupChats
        {
            get
            {
                return this.groupChats;
            }

            set
            {
                this.groupChats = value;
            }
        }

        public virtual ICollection<GroupMessage> GroupMessages
        {
            get
            {
                return this.groupMessages;
            }

            set
            {
                this.groupMessages = value;
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