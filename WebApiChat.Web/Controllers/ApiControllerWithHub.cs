using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApiChat.Web.Controllers
{
    using Microsoft.AspNet.Identity;

    public abstract class ApiControllerWithHub<THub> : BaseController
        where THub : IHub
    {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        public IHubContext HubContex
        {
            get { return hub.Value; }
        }

        //public string CurrentUserId
        //{
        //    get
        //    {
        //        return this.User.Identity.GetUserId();
        //    }   
        //}
    }
}
