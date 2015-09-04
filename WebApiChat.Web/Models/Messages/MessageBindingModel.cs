namespace WebApiChat.Web.Models.Messages
{
    #region

    using System.ComponentModel.DataAnnotations;

    #endregion

    public class MessageBindingModel
    {
        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [Display(Name = "Message text")]
        public string Text { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}