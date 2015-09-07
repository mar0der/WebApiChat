namespace WebApiChat.Web.Models.GroupMessage
{
    using WebApiChat.Models.Models;

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