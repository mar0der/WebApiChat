namespace WebApiChat.Models.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using WebApiChat.Models.Enums;

    public class MessageReceiver
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Message")]
        public int MessageId { get; set; }

        public virtual Message Message { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public virtual User Receiver { get; set; }

        [Required]
        public MessageStatusEnum Status { get; set; }
    }
}