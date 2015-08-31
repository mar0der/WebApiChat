using System.Diagnostics.Eventing.Reader;

namespace WebApiChat.Models.Models
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class Message
    {
        private ICollection<MessageReceiver> receivers;

        public Message()
        {
            this.receivers = new HashSet<MessageReceiver>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        // TODO return the attribute.
        //[Required]
        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        [ForeignKey("Chat")]
        public int ChatId { get; set; }

        public virtual Chat Chat { get; set; }

        public virtual ICollection<MessageReceiver> Receivers 
        {
            get
            {
                return this.receivers;
            }

            set
            {
                this.receivers = value;
            }
        }
    }
}