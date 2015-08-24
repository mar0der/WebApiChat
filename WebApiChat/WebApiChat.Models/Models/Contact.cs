namespace WebApiChat.Models.Models
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    #endregion

    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("ContactUser")]
        public string ContactUserId { get; set; }

        public virtual User ContactUser { get; set; }

        public bool IsBlocked { get; set; }
    }
}