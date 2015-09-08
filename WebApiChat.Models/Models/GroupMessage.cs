namespace WebApiChat.Models.Models
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class GroupMessage
    {
        private ICollection<User> recievedUsers;

        private ICollection<User> unRecievedUsers;

        public GroupMessage()
        {
            this.recievedUsers = new HashSet<User>();
            this.unRecievedUsers = new HashSet<User>();
        }
        
        [Key]
        public int Id { get; set; }

        public int GroupChatId { get; set; }

        public virtual GroupChat GroupChat { get; set; }

        public string Text { get; set; }

        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<User> RecievedUsers
        {
            get
            {
                return this.recievedUsers;
            }

            set
            {
                this.recievedUsers = value;
            }
        }

        public virtual ICollection<User> UnRecievedUsers
        {
            get
            {
                return this.unRecievedUsers;
            }

            set
            {
                this.unRecievedUsers = value;
            }
        }
    }
}