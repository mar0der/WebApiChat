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
            SeedUsersWithMessages(context);
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

                //context.Users.Add(userPesho);
                //context.Users.Add(userGosho);

                manager.Create(userGosho, "parola");
               



                var chat = new Chat();
                chat.Users.Add(userPesho);
                chat.Users.Add(userGosho);

                context.Chats.Add(chat);

                chat.Messages.Add(new Message()
                {
                    Sender = userPesho,
                    Text = "Whats up Gosho?!"
                });



                chat.Messages.Add(new Message()
                {
                    Sender = userGosho,
                    Text = "Chillin"
                });

                context.SaveChanges();

            }
        }
    }
}