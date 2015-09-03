using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApiChat.Models.Models;

namespace WebApiChat.Data.Migrations
{
    #region

    using System.Data.Entity.Migrations;

    #endregion

    internal sealed class Configuration : DbMigrationsConfiguration<WebApiChatDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WebApiChatDbContext context)
        {
           // SeedUsersWithMessages(context);
        }

        private void SeedUsersWithMessages(WebApiChatDbContext context)
        {

            if (!context.Users.Any())
            {

                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);
                var userPesho = new User
                {
                    UserName = "pesho",
                    FirstName = "Pesho",
                    LastName = "Petrov",
                    Email = "pesho@peshev.com",
                };

                manager.Create(userPesho, "parola");

                var userGosho = new User
                {
                    UserName = "gosho",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Email = "goshoGoshev@gmail.com",
                };


                var userChocho = new User
                {
                    UserName = "chocho",
                    FirstName = "Chocho",
                    LastName = "Chochev",
                    Email = "chocho@gmail.com",
                };

                var userMinka = new User
                {
                    UserName = "minka",
                    FirstName = "Minka",
                    LastName = "Chocheva",
                    Email = "minka@gmail.com",
                };

                manager.Create(userMinka, "parola");
                manager.Create(userGosho, "parola");
                manager.Create(userChocho, "parola");



                //var chat = new PrivateChat();
                //chat.Users.Add(userPesho);
                //chat.Users.Add(userGosho);

                //context.PrivateChats.Add(chat);

                //chat.Messages.Add(new PrivateMessage()
                //{
                //    Sender = userPesho,
                //    Text = "Whats up Gosho?!"
                //});



                //chat.Messages.Add(new PrivateMessage()
                //{
                //    Sender = userGosho,
                //    Text = "Chillin"
                //});

                var contact = new Contact()
                {
                    User = userPesho,
                    ContactUser = userGosho,
                    IsBlocked = false
                };


                var contact2 = new Contact()
                {
                    User = userPesho,
                    ContactUser = userChocho,
                    IsBlocked = false
                };

                var contact3 = new Contact()
                {
                    User = userPesho,
                    ContactUser = userMinka,
                    IsBlocked = false
                };

                var contact4 = new Contact()
                {
                    User = userGosho,
                    ContactUser = userPesho,
                    IsBlocked = false
                };

                context.Contacts.Add(contact);
                context.Contacts.Add(contact2);
                context.Contacts.Add(contact3);
                context.Contacts.Add(contact4);

                context.SaveChanges();

            }
        }
    }
}
