namespace WebApiChat.Data
{
    #region

    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using WebApiChat.Data.Migrations;
    using WebApiChat.Models.Models;

    #endregion

    public class WebApiChatDbContext : IdentityDbContext<User>
    {
        public WebApiChatDbContext()
            : base("ChatConnection", false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WebApiChatDbContext, Configuration>());
        }

        public static WebApiChatDbContext Create()
        {
            return new WebApiChatDbContext();
        }
    }
}