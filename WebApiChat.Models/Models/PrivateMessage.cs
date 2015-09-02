using System.Diagnostics.Eventing.Reader;
using WebApiChat.Models.Enums;

namespace WebApiChat.Models.Models
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class PrivateMessage
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        //[Required]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        public virtual PrivateChat Chat { get; set; }

        public MessageStatus Status { get; set; }
    }
}