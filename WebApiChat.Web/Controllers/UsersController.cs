﻿namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Threading;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    using WebApiChat.Web.Hubs;
    using WebApiChat.Web.Models.Account;
    using System.Collections.Generic;

    #endregion

    [System.Web.Http.Authorize]
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

        [Route("userStatusUpdate")]
        [HttpPost]
        public IHttpActionResult UserStatusUpdate()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BaseHub>();

            hubContext.Clients.All.userLogged(new { Id = this.CurrentUserId, this.CurrentUser.UserName });

            return this.Ok(Thread.CurrentPrincipal.Identity.GetUserId());
        }

        [HttpGet]
        [Route("getUserInfoByUsername")]
        public IHttpActionResult GetUserInfoByUsername([FromUri]string username)
        {

            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return this.BadRequest("user was not found.");
            }

            var viewModel = new UserPreviewViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return this.Ok(viewModel);
        }
    }
}