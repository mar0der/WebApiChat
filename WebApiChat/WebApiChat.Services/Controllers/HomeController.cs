namespace WebApiChat.Services.Controllers
{
    #region

    using System.Web.Mvc;

    #endregion

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            this.ViewBag.Title = "Home Page";

            return this.View();
        }
    }
}