namespace WebApiChat.Models.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using WebApiChat.Models.Enums;

    public class Chat
    {
        private ICollection<User> users;

        private ICollection<Message> messages;

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