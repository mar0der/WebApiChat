namespace WebApiChat.Web.Controllers
{
    #region

    using System;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    using WebApiChat.Data;
    using WebApiChat.Data.Interfaces;
    using WebApiChat.Data.Repositories;
    using WebApiChat.Models.Models;

    #endregion

    public abstract class ApiControllerWithHub<THub> : ApiController
        where THub : IHub
    {
        private readonly Lazy<IHubContext> hub =
            new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        public ApiControllerWithHub()
        {
            this.Data = new WebApiChatData(new WebApiChatDbContext());
        }

        public IChatData Data { get; private set; }

        public IHubContext HubContex
        {
            get
            {
                return this.hub.Value;
            }
        }

        public string CurrentUserId
        {
            get
            {
                return this.User.Identity.GetUserId();
            }
        }

        public User CurrentUser
        {
            get
            {
                return this.Data.Users.Find(this.User.Identity.GetUserId());
            }
        }

        public string CurrentUserUserName
        {
            get
            {
                return this.User.Identity.GetUserName();
            }
        }
    }
}