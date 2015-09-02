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
        private ICollection<PrivateChat> chats;

        private ICollection<Contact> contacts;

        private ICollection<PrivateMessage> privateMessages;

        private ICollection<GroupChat> groupChats;

        private ICollection<GroupMessage> groupMessages; 

        public User()
        {
            this.privateMessages = new HashSet<PrivateMessage>();
            this.chats = new HashSet<PrivateChat>();
            this.contacts = new HashSet<Contact>();
            this.groupChats= new HashSet<GroupChat>();
            this.groupMessages = new HashSet<GroupMessage>();
        }


        public int Age { get; set; }

        // [Required]
        public string FirstName { get; set; }

        // [Required]
        public string LastName { get; set; }

        public virtual ICollection<PrivateMessage> Messages
        {
            get
            {
                return this.privateMessages;
            }

            set
            {
                this.privateMessages = value;
            }
        }

        public virtual ICollection<PrivateChat> Chats
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

        public virtual ICollection<GroupChat> GroupChats
        {
            get { return this.groupChats; }
            set { this.groupChats = value; }
        }

        public virtual ICollection<GroupMessage> GroupMessages
        {
            get { return this.groupMessages; }
            set { this.groupMessages = value; }
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