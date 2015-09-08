namespace WebApiChat.Models.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using WebApiChat.Models.Enums;

    public class GroupMessageReceiver
    {
        [Key, Column(Order = 0)]
        public int GroupMessageId { get; set; }

        public virtual GroupMessage GroupMessage { get; set; }

        [Key, Column(Order = 1)]
        public string ReceiverId { get; set; }

        public virtual User Receiver { get; set; }

        public MessageStatus Status { get; set; }
    }
}