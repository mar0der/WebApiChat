using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebApiChat.Data;
using WebApiChat.Data.Interfaces;
using WebApiChat.Data.Repositories;
using WebApiChat.Models.Models;

namespace WebApiChat.Web.Controllers
{
    using Microsoft.AspNet.Identity;

    public abstract class ApiControllerWithHub<THub> : ApiController
        where THub : IHub
    {
        private IChatData data;

        public ApiControllerWithHub()
        {
            this.data = new WebApiChatData(new WebApiChatDbContext());
        }

        Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        public IChatData Data
        {
            get { return this.data; }
        }

        public IHubContext HubContex
        {
            get { return hub.Value; }
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
