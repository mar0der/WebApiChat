using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;

namespace WebApiChat.Web
{
    using System.Collections.Generic;

    public class ConnectedUser
    {

        public string Name { get; set; }
        //public HashSet<string> ConnectionIds { get; set; }

        public string ConnectionsIds { get; set; }

        public string Id { get; set; }
    }

    public static class ConnectionManager
    {
        

        private static readonly ConcurrentDictionary<string, ConnectedUser> ConnectedUsers
          = new ConcurrentDictionary<string, ConnectedUser>(StringComparer.InvariantCultureIgnoreCase);

        public static ConcurrentDictionary<string, ConnectedUser> Users
        {
            get { return ConnectedUsers; }
        }


        //public static IEnumerable<string> GetConnectedUsers(Microsoft.AspNet.SignalR.Hubs.HubCallerContext Context)
        //{

        //    return ConnectedUsers.Where(x =>
        //    {

        //        lock (x.Value.ConnectionsIds)
        //        {

        //            return !x.Value.ConnectionsIds.Contains(Context.ConnectionId, StringComparer.InvariantCultureIgnoreCase);
        //        }

        //    }).Select(x => x.Key);
        //}




        //public static HashSet<string> GetUserConnection(string username)
        //{
        //    ConnectedUser user;
        //    ConnectedUsers.TryGetValue(username, out user);

        //    return user.ConnectionsIds;
        //}


        //private static Dictionary<string, string> applicationUsers = new Dictionary<string, string>();

        //public static void Add(string name, string connectionId)
        //{
        //    if (applicationUsers.ContainsKey(name))
        //    {
        //        applicationUsers[name] = connectionId;
        //    }
        //    else
        //    {
        //        applicationUsers.Add(name, connectionId);
        //    }
        //}

        //public static void Remove(string name, string connectionId)
        //{
        //    applicationUsers.Remove(name);
        //}

        //public static string GetCurrentConnection()
        //{
        //    if (applicationUsers.Count == 0)
        //    {
        //        return null;
        //    }
          
        //    return applicationUsers.Values.Last();
        //}

        //public static Dictionary<string, string> DictUses
        //{
        //    get { return applicationUsers; }
        //}

       
    }
}