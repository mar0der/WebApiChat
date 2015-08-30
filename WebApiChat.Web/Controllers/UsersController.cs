namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;

    using WebApiChat.Web.Hubs;

    #endregion

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
            var contacts =
                this.Data.Contacts.All()
                    .Where(c => c.UserId == currentUserId)
                    .ToList()
                    .Select(
                        c =>
                        new
                            {
                                Id = c.ContactUserId, 
                                Name = c.ContactUser.UserName, 
                                IsOnline = connectedUsers.Any(cu => cu.Id == c.ContactUserId)
                            });

            return this.Ok(contacts);
        }
    }
}