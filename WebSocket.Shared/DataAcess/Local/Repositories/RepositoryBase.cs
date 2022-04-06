using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess.Local.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : BaseEntity
    {
        public SalkySqlLiteDbContext dbcontext;
        public RepositoryBase(SalkySqlLiteDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public virtual IQueryable<T> GetQuery()
        {
            throw new NotImplementedException($"{nameof(GetQuery)} at repository of {typeof(T).Name} must be overrided");
        }

        public bool ToTrack { get; private set; } = false;

        public IRepository<T> AsTracking()
        {
            this.ToTrack = false;
            return this;
        }

        public IRepository<T> AsNoTracking()
        {
            this.ToTrack = false;
            return this;
        }

        public T? GetById(int id)
        {
            return GetQuery().Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<T>? GetAll()
        {
            return GetQuery().ToList();
        }

        public T? Find(Func<T, bool> expression)
        {
            return GetQuery().Where(expression).FirstOrDefault();
        }

        public void Update(T entity)
        {
            dbcontext.Update(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Remove(T entity)
        {
            dbcontext.Remove(entity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        }

        public void Remove(int id)
        {
            var entity = GetById(id);
            Remove(entity);
        }

        public T Add(T entity)
        {
            dbcontext.Add(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            return entity;
        }

        public int SaveChanges()
        {
            return dbcontext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbcontext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbcontext.SaveChanges();
            dbcontext.Dispose();
        }
    }
}
