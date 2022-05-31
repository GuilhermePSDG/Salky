using Salky.Domain.Contexts;
using Salky.Domain.Contracts;
using Salky.Domain.Exceptions;

namespace Salky.Domain.Repositories
{

    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public SalkyDbContext db;

        public RepositoryBase(SalkyDbContext dbctx)
        {
            db = dbctx;
        }

        public virtual void Add(T entity)
        {
            db.Add(entity);
        }
        public virtual void Remove(T entity)
        {
            db.ChangeTracker.Clear();
            db.Remove(entity);
        }
        public virtual void Update(T entity)
        {
            db.ChangeTracker.Clear();
            db.Update(entity);
        }
        public async Task<int> EnsureSaveChangesAsync()
        {
            var c = await this.SaveChangesAsync();
            if (c == 0)
                throw new UnableToSaveChangesException();
            return c;
        }

        public async Task<int> SaveChangesAsync()
        {
            var saveCount = await db.SaveChangesAsync();
            db.ChangeTracker.Clear();
            return saveCount;
        }
    }
}
