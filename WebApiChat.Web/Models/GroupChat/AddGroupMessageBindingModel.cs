using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiChat.Web.Models.GroupChat
{
    public class AddGroupMessageBindingModel
    {
        public string Text { get; set; }

        public int GroupId { get; set; }
    }
}