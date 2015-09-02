using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiChat.Web.Models.GroupChat
{
    public class GroupChatBindingModel
    {
        // TODO add validations 

        public string GroupName { get; set; }
        public string[] UserIds { get; set; }
    }
}