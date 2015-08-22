namespace WebApiChat.Data
{
    #region

    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using WebApiChat.Data.Migrations;
    using WebApiChat.Models.Models;

    #endregion

    public class ChatDbContext : IdentityDbContext<User>
    {
        public ChatDbContext()
            : base("ChatConnection", false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChatDbContext, Configuration>());
        }

        public static ChatDbContext Create()
        {
            return new ChatDbContext();
        }
    }
}