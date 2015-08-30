namespace WebApiChat.Web.Controllers
{
    #region

    using System.Web.Http;

    using WebApiChat.Web.Hubs;

    #endregion

    public class ConController : ApiControllerWithHub<BaseHub>
    {
        public IHttpActionResult GetAllFriends()
        {
            return this.Ok();
        }
    }
}