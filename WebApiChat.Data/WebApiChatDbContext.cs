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

        public IDbSet<Chat> Chats { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<Contact> Contacts { get; set; }

        public static WebApiChatDbContext Create()
        {
            return new WebApiChatDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Contacts).WithRequired(c => c.User);

            modelBuilder.Entity<Contact>().HasRequired(c => c.User);

            base.OnModelCreating(modelBuilder);
        }
    }
}