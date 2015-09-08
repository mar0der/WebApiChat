namespace WebApiChat.Web
{
    #region

    using System;
    using System.Collections.Concurrent;

    #endregion

    public class ConnectedUser
    {
        public string Name { get; set; }

        // public HashSet<string> ConnectionIds { get; set; }
        public string ConnectionsIds { get; set; }

        public string Id { get; set; }
    }

    public static class ConnectionManager
    {
        private static readonly ConcurrentDictionary<string, ConnectedUser> ConnectedUsers =
            new ConcurrentDictionary<string, ConnectedUser>(StringComparer.InvariantCultureIgnoreCase);

        public static ConcurrentDictionary<string, ConnectedUser> Users
        {
            get
            {
                return ConnectedUsers;
            }
        }
    }
}