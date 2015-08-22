namespace WebApiChat.Data.Interfaces
{
    using WebApiChat.Models.Models;

    public interface IChatData
    {
        IRepository<User> Users { get;}

        IRepository<Message> Messages { get;}

        int SaveChanges();
    }
}