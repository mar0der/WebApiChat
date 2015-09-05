using System.Linq;
using Microsoft.AspNet.Identity;
using WebApiChat.Web.Hubs;
using System.Web.Http;
using System.Threading;

namespace WebApiChat.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerWithHub<BaseHub>
    {
        public IHttpActionResult GetOnlineUsers()
        {
            var connectedUsers = ConnectionManager.Users.Values;

            return this.Ok(connectedUsers);
        }

        [Route("friends")]
        public IHttpActionResult GetAllFriends()
        {

            var connectedUsers = ConnectionManager.Users.Values;
            var currentUserId = this.User.Identity.GetUserId();

            var contats = this.Data.Contacts.All().Where(c => c.UserId == currentUserId).ToList();
            var contacts = this.Data.Contacts
                .All()
                .Where(c => c.UserId == currentUserId).ToList()
                  .Select(c => new
                  {
                      Id = c.ContactUserId,
                      Name = c.ContactUser.UserName,
                      IsOnline = connectedUsers.Any(cu => cu.Id == c.ContactUserId)
                  });

            return this.Ok(contacts);
        }

        [Route("userStatusUpdate")]
        [HttpPost]
        public IHttpActionResult UserStatusUpdate()
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<BaseHub>();

            hubContext.Clients.All.userLogged(new
            {
                Id = this.CurrentUserId,
                UserName = this.CurrentUser.UserName
            });

            return this.Ok(Thread.CurrentPrincipal.Identity.GetUserId());
        }
    }
}
