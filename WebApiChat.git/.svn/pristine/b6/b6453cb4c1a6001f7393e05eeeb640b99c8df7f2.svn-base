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
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        [Required]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        public virtual Chat Chat { get; set; }
    }
}