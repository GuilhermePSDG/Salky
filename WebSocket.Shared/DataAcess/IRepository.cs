using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess
{
    public interface IRepository<T> : IDisposable  where T : BaseEntity
    {
        //
        public IRepository<T> AsTracking();
        public IRepository<T> AsNoTracking();
        //
        public T? GetById(int id);
        public List<T>? GetAll();
        public T? Find(Func<T, bool> expression);
        //
        public void Update(T entity);
        //
        public void Remove(T entity);
        public void Remove(int id);
        //
        public T Add(T entity);
        //
        public int SaveChanges();
        public Task<int> SaveChangesAsync();

    }
}
