namespace WebApiChat.Web.Models.Messages
{
    #region

    using System;
    using System.Linq.Expressions;

    using WebApiChat.Models.Models;

    #endregion

    public class MessageViewModel
    {
        public static Expression<Func<Message, MessageViewModel>> ViewModel
        {
            get
            {
                return m => new MessageViewModel { Id = m.Id, Text = m.Text, SenderId = m.SenderId, ChatId = m.ChatId };
            }
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public string SenderId { get; set; }

        public int ChatId { get; set; }
    }
}