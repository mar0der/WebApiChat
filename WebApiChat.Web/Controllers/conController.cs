using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using WebApiChat.Web.Hubs;

namespace WebApiChat.Web.Controllers
{
    public class ConController : ApiControllerWithHub<BaseHub>
    {
        public IHttpActionResult GetAllFriends()
        {
            
            return this.Ok();
        }
    }
}
