namespace WebApiChat.Models.Models
{
    #region

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WebApiChat.Models.Enums;

    #endregion

    public class Chat
    {
        private ICollection<Message> messages;

        private ICollection<User> users;

        public Chat()
        {
            this.users = new HashSet<User>();
            this.messages = new HashSet<Message>();
        }

        [Key]
        public int Id { get; set; }

        public ChatType ChatType { get; set; }

        public virtual ICollection<User> Users
        {
            get
            {
                return this.users;
            }

            set
            {
                this.users = value;
            }
        }

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
    }
}