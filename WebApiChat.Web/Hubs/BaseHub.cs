namespace WebApiChat.Web.Hubs
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.Cors;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Microsoft.AspNet.SignalR.Authorize]
    public class BaseHub : Hub
    {
        public override Task OnConnected()
        {
            //
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;
            string id = Context.User.Identity.GetUserId();

            var user = ConnectionManager.Users.GetOrAdd(userName, _ => new ConnectedUser
            {
                Name = userName,
                ConnectionsIds = connectionId,
                Id = id
            });

            lock (user.ConnectionsIds)
            {

                user.ConnectionsIds = connectionId;

                // // broadcast this to all clients other than the caller
                // Clients.AllExcept(user.ConnectionIds.ToArray()).userConnected(userName);

                // Or you might want to only broadcast this info if this 
                // is the first connection of the user
                if (user.ConnectionsIds != null || user.ConnectionsIds!= string.Empty)
                {

                    Clients.Others.userConnected(userName);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {

            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            ConnectedUser user;
            ConnectionManager.Users.TryGetValue(userName, out user);

            if (user != null)
            {

                lock (user.ConnectionsIds)
                {

                    user.ConnectionsIds = null;

                    if (String.IsNullOrEmpty(user.ConnectionsIds))
                    {

                        ConnectedUser removedUser;
                        ConnectionManager.Users.TryRemove(userName, out removedUser);

                        // You might want to only broadcast this info if this 
                        // is the last connection of the user and the user actual is 
                        // now disconnected from all connections.
                        Clients.Others.userDisconnected(userName);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }




    //    public static ConnectionMapping<string> connectedUsers = new ConnectionMapping<string>();

    //    //public WebApiChatData Data;
   
    //    public void GetOnlineUsers()
    //    {

    //        var users = ConnectionManager.DictUses.ToList();

    //        var currentUserConnectionId = Context.ConnectionId;

    //    }

    //    public override Task OnConnected()
    //    {
    //        string name = Context.User.Identity.Name;

    //        string userId = Context.User.Identity.GetUserId();

    //        //connectedUsers.Add(userId, Context.ConnectionId);

    //        ConnectionManager.Add(name, Context.ConnectionId);

    //        return base.OnConnected();
    //        ConnectionManager.GetConnectedUsers(Context);
    //    }

    //    //login join 
    //    public void Join()
    //    {
    //        var name = Context.User.Identity.GetUserName();
    //        var userId = Context.User.Identity.GetUserId();

    //        var connectionId = Context.ConnectionId;




    //        Clients.All.join(new
    //        {
    //            Key = name,
    //            Value = userId,
    //            ConnectionId = connectionId
    //        });
    //    }

    //    public override Task OnDisconnected(bool stopCalled)
    //    {
    //        string name = Context.User.Identity.Name;

    //        var id = Context.ConnectionId;
    //        connectedUsers.Remove(name, Context.ConnectionId);

    //        ConnectionManager.Remove(name, id);

    //        return base.OnDisconnected(stopCalled);
    //    }

    //    public override Task OnReconnected()
    //    {
    //        string name = Context.User.Identity.Name;

    //        if (!connectedUsers.GetConnections(name).Contains(Context.ConnectionId))
    //        {
    //            connectedUsers.Add(name, Context.ConnectionId);
    //            ConnectionManager.Add(name, Context.ConnectionId);
    //        }

    //        return base.OnReconnected();
    //    }



    //    public void SendMessage(string message, string recieverUsername)
    //    {
    //        Clients.All.sendMessage("hello");
    //    }
    }
}