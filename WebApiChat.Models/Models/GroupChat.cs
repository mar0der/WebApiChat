namespace WebApiChat.Models.Models
{
    #region

    using System.Collections.Generic;

    #endregion

    public class GroupChat
    {
        private ICollection<GroupMessage> groupMessageses;

        private ICollection<User> users;

        public GroupChat()
        {
            this.users = new HashSet<User>();
            this.groupMessageses = new HashSet<GroupMessage>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

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

        public virtual ICollection<GroupMessage> GroupMessages
        {
            get
            {
                return this.groupMessageses;
            }

            set
            {
                this.groupMessageses = value;
            }
        }
    }
}