namespace WebApiChat.Data.Interfaces
{
    #region

    using WebApiChat.Models.Models;

    #endregion

    public interface IChatData
    {
        IRepository<User> Users { get; }

        IRepository<Message> Messages { get; }

        IRepository<Chat> Chats { get; }

        IRepository<Contact> Contacts { get; }

        int SaveChanges();
    }
}