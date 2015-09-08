namespace WebApiChat.Web.Models.GroupMessage
{
    #region

    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using WebApiChat.Models.Models;

    #endregion

    public class GroupMessageViewModel
    {
        public int Id { get; set; }

        public string Sender { get; set; }

        public string Text { get; set; }

        public int GroupId { get; set; }

        public static Expression<Func<GroupMessage, GroupMessageViewModel>> ViewModel
        {
            get
            {
                return
                    gm =>
                    new GroupMessageViewModel
                        {
                            Id = gm.Id, 
                            GroupId = gm.GroupChatId, 
                            Sender = gm.Sender.UserName, 
                            Text = gm.Text
                        };
            }
        }

        public static GroupMessageViewModel CreateOne(GroupMessage message)
        {
            return new GroupMessageViewModel
                       {
                           GroupId = message.GroupChatId, 
                           Id = message.Id, 
                           Text = message.Text, 
                           Sender = message.Sender.UserName
                       };
        }
    }
}