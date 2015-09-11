namespace WebApiChat.Models.Models
{
    #region

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using WebApiChat.Models.Enums;

    #endregion

    public class PrivateMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public virtual User Receiver { get; set; }

        public bool IsFileLink { get; set; }

        public MessageStatus Status { get; set; }
    }
}