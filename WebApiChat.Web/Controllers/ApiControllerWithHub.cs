namespace WebApiChat.Web.Controllers
{
    #region

    using System;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    #endregion

    public abstract class ApiControllerWithHub<THub> : BaseController
        where THub : IHub
    {
        private readonly Lazy<IHubContext> hub =
            new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        public IHubContext HubContex
        {
            get
            {
                return this.hub.Value;
            }
        }
    }
}