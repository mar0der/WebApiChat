namespace WebApiChat.Data.Repositories
{
    #region

    using System.Data.Entity;
    using System.Linq;

    using WebApiChat.Data.Interfaces;

    #endregion

    public class EfRepository<T> : IRepository<T>
        where T : class
    {
        private readonly DbContext context;

        private readonly IDbSet<T> set;

        public EfRepository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return this.set;
        }

        public T Find(object id)
        {
            return this.set.Find(id);
        }

        public void Add(T entry)
        {
            this.ChangeState(entry, EntityState.Added);
        }

        public void Update(T entry)
        {
            this.ChangeState(entry, EntityState.Modified);
        }

        public T Delete(T entry)
        {
            this.ChangeState(entry, EntityState.Deleted);
            return entry;
        }

        public void Delete(object id)
        {
            var entity = this.Find(id);
            this.Delete(entity);
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}