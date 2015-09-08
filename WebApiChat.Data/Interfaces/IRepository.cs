namespace WebApiChat.Data.Interfaces
{
    #region

    using System.Linq;

    #endregion

    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> All();

        T Find(object id);

        void Add(T entry);

        void Update(T entry);

        T Delete(T entity);

        void Delete(object id);

        void SaveChanges();
    }
}