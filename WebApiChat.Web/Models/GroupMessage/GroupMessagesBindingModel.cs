namespace WebApiChat.Web.Models.GroupMessage
{
    public class GroupMessagesBindingModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SenderId { get; set; }

        public string SenderUsername { get; set; }

        public string Text { get; set; }
    }
}