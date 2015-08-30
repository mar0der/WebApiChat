namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using WebApiChat.Web.Models.Messages;

    #endregion

    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/messages")]
    public class MessagesController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            var messages = this.Data.Messages.All().Select(MessageViewModel.ViewModel);

            return this.Ok(messages);
        }
    }
}