namespace WebApiChat.Web.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;

    using WebApiChat.Data;
    using WebApiChat.Data.Interfaces;
    using WebApiChat.Data.Repositories;
    using WebApiChat.Models.Models;

    #endregion

    public class MessagesController : ApiController
    {
        public MessagesController()
            : this(new WebApiChatData(new WebApiChatDbContext()))
        {
        }

        public MessagesController(IChatData data)
        {
            this.Data = data;
        }

        public IChatData Data { get; set; }

        [HttpGet]
        public IHttpActionResult Get()
        {
            //var message = new Message
            //                  {
            //                      Text = "hi", 
            //                      SenderId = "d51f5c5d-827e-48e7-827e-1a056b6fa749", 
            //                      ReceiverId = "d51f5c5d-827e-48e7-827e-1a056b6fa749"
            //                  };
            //this.Data.Messages.Add(message);
            //this.Data.SaveChanges();
            var db = new WebApiChatDbContext();

            var msg = this.Data.Contacts.Find(2);
           // msg.
            
            var usersCount = this.Data.Users.All().Count();
            return this.Ok(usersCount);
        }
    }
}