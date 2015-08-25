namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using WebApiChat.Data;
    using WebApiChat.Data.Interfaces;
    using WebApiChat.Data.Repositories;
    using WebApiChat.Models.Models;
    using WebApiChat.Web.Models.Messages;

    #endregion

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/messages")]
    public class MessagesController : BaseController
    {
        //public MessagesController()
        //    : this(new WebApiChatData(new WebApiChatDbContext()))
        //{
        //}

        //public MessagesController(IChatData data)
        //{
        //    this.Data = data;
        //}

        //public IChatData Data { get; set; }

        [HttpGet]
        [Authorize]
        public IHttpActionResult Get()
        {
            var messages = this.Data.Messages.All().Select(MessageViewModel.ViewModel);

            return this.Ok(messages);
        }
    }
}