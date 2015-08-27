using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using WebApiChat.Web.Hubs;

namespace WebApiChat.Web.Controllers
{
    public class ContactsController : BaseController
    {
        [System.Web.Http.Authorize]
        public IHttpActionResult GetContacts()
        {
            var currentUserId = this.User.Identity.GetUserId();
           // var courrentUserId = this.User.T


            var contacts = this.Data.Contacts.All().Where(c => c.UserId == currentUserId);

            // TODO create view for contats


            var context = GlobalHost.ConnectionManager.GetHubContext<BaseHub>();

            var clients = context.Clients.All;


            //foreach (var client in clients)
            //{
            //    Console.WriteLine(client.ToString());
            //}

            return this.Ok(contacts.Select(c=> new
            {
                ContactsOwer = c.ContactUser.UserName,
                User = c.ContactUser.UserName,
                ContactUserId = c.ContactUserId
            }));
        }
    }
}