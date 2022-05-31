namespace Salky.Domain.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        public void Add(T entity);
        public void Remove(T entity);
        public void Update(T entity);
        public Task<int> EnsureSaveChangesAsync();
        public Task<int> SaveChangesAsync();
    }
}
