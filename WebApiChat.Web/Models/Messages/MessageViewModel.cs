namespace WebApiChat.Web.Models.Messages
{
    #region

    using System;
    using System.Linq.Expressions;

    using WebApiChat.Models.Models;

    #endregion

    public class MessageViewModel
    {
        public static Expression<Func<PrivateMessage, MessageViewModel>> ViewModel
        {
            get
            {
                return m => new MessageViewModel { Id = m.Id, Text = m.Text, SenderId = m.SenderId };
            }
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public string SenderId { get; set; }

        public int ChatId { get; set; }
    }
}