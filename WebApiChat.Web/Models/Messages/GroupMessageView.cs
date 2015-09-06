using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using WebApiChat.Models.Models;

namespace WebApiChat.Web.Models.Messages
{
    public class GroupMessageView
    {
        public int Id { get; set; }

        public string SenderUsername { get; set; }

        public string Text { get; set; }

        public int  GroupId { get; set; }

        public static GroupMessageView CreateOne(GroupMessage message)
        {
            return new GroupMessageView()
            {
                GroupId = message.GroupChatId,
                Id =  message.Id,
                Text=  message.Text,
                SenderUsername = message.Sender.UserName
            };
        }
    }
}