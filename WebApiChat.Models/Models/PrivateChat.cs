namespace WebApiChat.Models.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WebApiChat.Models.Enums;

    public class PrivateChat
    {
        private ICollection<PrivateMessage> messages;

        private ICollection<User> users;

       

        public PrivateChat()
        {
            this.messages = new HashSet<PrivateMessage>();
            this.users = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }

        public virtual ICollection<PrivateMessage> Messages
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