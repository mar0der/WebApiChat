namespace WebApiChat.Data.Interfaces
{
    #region

    using WebApiChat.Models.Models;

    #endregion

    public interface IChatData
    {
        IRepository<User> Users { get; }

        IRepository<PrivateMessage> Messages { get; }

        IRepository<Contact> Contacts { get; }

        IRepository<GroupChat> GroupChats { get; }

        IRepository<GroupMessage> GroupMessages { get; }

        IRepository<GroupMessageReceiver> GroupMessageReceivers { get; }

        int SaveChanges();
    }
}