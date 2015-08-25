using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace WebApiChat.Web.Controllers
{
    public class ContactsController : BaseController
    {
        [Authorize]
        public IHttpActionResult GetContacts()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var contacts = this.Data.Contacts.All().Where(c => c.UserId == currentUserId);

            // TODO create view for contats

            return this.Ok(contacts.Select(c=> new
            {
                ContactsOwer = c.ContactUser.UserName,
                User = c.ContactUser.UserName,
                ContactUserId = c.ContactUserId
            }));
        }
    }
}