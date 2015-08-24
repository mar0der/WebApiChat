namespace WebApiChat.Web.Hubs
{
    #region

    using Microsoft.AspNet.SignalR;

    #endregion

    public class MessagesHub : Hub
    {
        public void Hello(string message)
        {
            this.Clients.All.messageReceived(message);
        }
    }
}