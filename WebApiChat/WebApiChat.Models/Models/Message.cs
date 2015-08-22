namespace WebApiChat.Models.Models
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        public string Text { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public User Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public User Receiver { get; set; }
    }
}