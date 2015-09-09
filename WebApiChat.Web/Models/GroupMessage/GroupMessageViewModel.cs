namespace WebApiChat.Web.Models.GroupMessage
{
    #region

    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using WebApiChat.Models.Models;
    using WebApiChat.Models.Enums;
    using System.Web.Routing;
    using System.Web;

    #endregion

    public class GroupMessageViewModel
    {
        public int Id { get; set; }

        public string Sender { get; set; }

        public string Text { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<string> SeenBy { get; set; }

        public static Expression<Func<GroupMessage, GroupMessageViewModel>> ViewModel
        {
            get
            {
                return
                    gm =>
                    new GroupMessageViewModel()
                        {
                            Id = gm.Id, 
                            GroupId = gm.GroupChatId, 
                            Sender = gm.Sender.UserName,
                            Text = gm.Text,
                            SeenBy = gm
                            .GroupMessageReceivers.Where(mr => mr.Status == MessageStatus.Seen
                                && mr.Receiver.UserName != HttpContext.Current.User.Identity.Name)
                            .Select(mr => mr.Receiver.FirstName)
                        };
            }
        }

        public static GroupMessageViewModel CreateOne(GroupMessage message, string currentUserId)
        {
            return new GroupMessageViewModel
                       {
                           GroupId = message.GroupChatId, 
                           Id = message.Id, 
                           Text = message.Text,
                           Sender = message.Sender.UserName,
                           SeenBy = message
                           .GroupMessageReceivers.Where(mr => mr.Status == MessageStatus.Seen
                               && mr.ReceiverId != currentUserId)
                           .Select(mr => mr.Receiver.FirstName)
                       };
        }
    }
}