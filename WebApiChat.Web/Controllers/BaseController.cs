using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiChat.Data;
using WebApiChat.Data.Interfaces;
using WebApiChat.Data.Repositories;

namespace WebApiChat.Web.Controllers
{
    public class BaseController : ApiController
    {

        public BaseController()
            : this(new WebApiChatData(new WebApiChatDbContext()))
        {
        }

        public BaseController(IChatData data)
        {
            this.Data = data;
        }
        public IChatData Data { get; set; }
    }
}
