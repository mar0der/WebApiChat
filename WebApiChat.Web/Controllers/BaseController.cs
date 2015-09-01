namespace WebApiChat.Web.Controllers
{
    #region

    using System.Web.Http;

    using Microsoft.AspNet.Identity;

    using WebApiChat.Data;
    using WebApiChat.Data.Interfaces;
    using WebApiChat.Data.Repositories;

    #endregion

    public class BaseController : ApiController
    {
        public BaseController()
            : this(new WebApiChatData(new WebApiChatDbContext()))
        {
        }

        public BaseController(IChatData data)
        {
            this.Data = data;
        }

        public IChatData Data { get; set; }

        public string CurrentUserId
        {
            get
            {
                return this.User.Identity.GetUserId();
            }
        }

        public string CurrentUsername
        {
            get
            {
                return this.User.Identity.GetUserName();
            }
        }
    }
}