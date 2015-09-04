using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiChat.Web.Models
{
    public class GroupMessagesBindingModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SenderId { get; set; }

        public string SenderUsername { get; set; }
    }
}