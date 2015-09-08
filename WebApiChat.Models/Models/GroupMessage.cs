namespace WebApiChat.Models.Models
{
    #region

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    #endregion

    public class GroupMessage
    {
        private ICollection<GroupMessageReceiver> groupMessageReceivers;

        public GroupMessage()
        {
            this.groupMessageReceivers = new HashSet<GroupMessageReceiver>();
        }
            
        [Key]
        public int Id { get; set; }

        public int GroupChatId { get; set; }

        public virtual GroupChat GroupChat { get; set; }

        public string Text { get; set; }

        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<GroupMessageReceiver> GroupMessageReceivers
        {
            get
            {
                return this.groupMessageReceivers;
            }

            set
            {
                this.groupMessageReceivers = value;
            }
        }
    }
}