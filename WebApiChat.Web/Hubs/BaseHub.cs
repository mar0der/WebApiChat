using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Eaf.InfrastructureLayer.Communication.SignalR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using WebApiChat.Data;
using WebApiChat.Data.Repositories;
using WebApiChat.Models.Models;

namespace WebApiChat.Web.Hubs
{
    [Authorize]
    public class BaseHub : Hub
    {

        //private WebApiChatData Data;
        //public BaseHub()
        //{
        //    this.Data = new WebApiChatData(new WebApiChatDbContext());
        //}

        public static ConnectionMapping<string> connectedUsers = new ConnectionMapping<string>();
        
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            string userId = Context.User.Identity.GetUserId();

            connectedUsers.Add(userId, Context.ConnectionId);
          
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;

            connectedUsers.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!connectedUsers.GetConnections(name).Contains(Context.ConnectionId))
            {
                connectedUsers.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }



        public void SendMessage(string message, string recieverUsername)
        {
            Clients.All.sendMessage("hello");



        }

    }
}