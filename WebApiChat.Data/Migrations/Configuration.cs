namespace WebApiChat.Data.Migrations
{
    #region

    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using WebApiChat.Models.Models;

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
            this.SeedUsersWithMessages(context);
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
                                        Email = "pesho@peshev.com"
                                    };

                var userGosho = new User
                                    {
                                        UserName = "gosho", 
                                        FirstName = "Gosho", 
                                        LastName = "Goshev", 
                                        Email = "goshoGoshev@gmail.com"
                                    };

                var userChocho = new User
                                     {
                                         UserName = "chocho", 
                                         FirstName = "Chocho", 
                                         LastName = "Chochev", 
                                         Email = "chocho@gmail.com"
                                     };

                var userMinka = new User
                                    {
                                        UserName = "minka", 
                                        FirstName = "Minka", 
                                        LastName = "Chocheva", 
                                        Email = "minka@gmail.com"
                                    };

                manager.Create(userPesho, "parola");
                manager.Create(userMinka, "parola");
                manager.Create(userGosho, "parola");
                manager.Create(userChocho, "parola");

                context.PrivateMessages.Add(
                    new PrivateMessage { Sender = userPesho, Receiver = userGosho, Text = "Whats up Gosho?!" });

                context.PrivateMessages.Add(
                    new PrivateMessage { Sender = userGosho, Receiver = userPesho, Text = "Chillin" });

                var contact = new Contact { User = userPesho, ContactUser = userGosho, IsBlocked = false };

                var contact2 = new Contact { User = userPesho, ContactUser = userChocho, IsBlocked = false };

                var contact3 = new Contact { User = userPesho, ContactUser = userMinka, IsBlocked = false };

                var contact4 = new Contact { User = userGosho, ContactUser = userPesho, IsBlocked = false };

                var contact5 = new Contact { User = userMinka, ContactUser = userPesho, IsBlocked = false };

                var contact6 = new Contact { User = userChocho, ContactUser = userPesho, IsBlocked = false };

                var group = new GroupChat { Name = "grupichkata mi" };
                group.Users.Add(userPesho);
                group.Users.Add(userGosho);
                group.Users.Add(userMinka);

                var contact7 = new Contact { User = userMinka, ContactUser = userGosho };

                var groupMessage1 = new GroupMessage
                                        {
                                            GroupChat = group, 
                                            Date = DateTime.Now, 
                                            Sender = userPesho, 
                                            Text = "maraba grupa"
                                        };

                context.GroupChats.Add(group);
                context.GroupMessages.Add(groupMessage1);
                context.Contacts.Add(contact7);
                context.Contacts.Add(contact);
                context.Contacts.Add(contact2);
                context.Contacts.Add(contact3);
                context.Contacts.Add(contact4);
                context.Contacts.Add(contact5);
                context.Contacts.Add(contact6);

                context.SaveChanges();
            }
        }
    }
}