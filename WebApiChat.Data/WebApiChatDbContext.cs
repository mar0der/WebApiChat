using System;

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

        public IDbSet<PrivateChat> PrivateChats { get; set; }

        public IDbSet<PrivateMessage> PrivateMessages { get; set; }

        public IDbSet<Contact> Contacts { get; set; }

        public IDbSet<GroupChat> GroupChats { get; set; }

        public IDbSet<GroupMessage> GroupMessages { get; set; }

        public static WebApiChatDbContext Create()
        {
            return new WebApiChatDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Contacts).WithRequired(c => c.User);

            modelBuilder.Entity<Contact>().HasRequired(c => c.User);

            modelBuilder.Entity<User>().HasMany(g => g.GroupMessages).WithMany(gm => gm.UnRecievedUsers);
            modelBuilder.Entity<User>().HasMany(g => g.GroupMessages).WithMany(gm => gm.RecievedUsers);

            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<PrivateMessage>()
                .HasRequired(pm => pm.Reciever).WithMany(u => u.Messages).HasForeignKey(c => c.RecieverId).WillCascadeOnDelete(false);


            modelBuilder.Entity<User>()
                .HasMany(u => u.Messages).WithRequired(m => m.Reciever).HasForeignKey(x=>x.RecieverId).WillCascadeOnDelete(false);

            //modelBuilder.Entity<PrivateMessage>().HasRequired(x => x.Reciever).WithMany(u => u.Messages).WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()k
            

            
            base.OnModelCreating(modelBuilder);

        }
    }
}