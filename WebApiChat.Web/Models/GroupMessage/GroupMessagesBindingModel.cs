namespace WebApiChat.Web.Models.GroupMessage
{
    using System.ComponentModel.DataAnnotations;

    public class GroupMessagesBindingModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [MinLength(1)]
        [Required]
        public string Text { get; set; }
    }
}