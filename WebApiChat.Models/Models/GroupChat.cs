using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiChat.Models.Models
{
    public class GroupChat
    {
        private ICollection<User> users;

        private ICollection<GroupMessage> groupMessageses;

        public GroupChat()
        {
            this.users = new HashSet<User>();
            this.groupMessageses = new HashSet<GroupMessage>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
            set { this.users = value; }
        }


        public virtual ICollection<GroupMessage> GroupMessages
        {
            get { return this.groupMessageses; }
            set { this.groupMessageses = value; }
        }
    }
}
