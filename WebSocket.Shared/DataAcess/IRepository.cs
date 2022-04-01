using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess.Local.Services
{
    public interface IRepository<T> where T : BaseEntity
    {
        //
        public T? GetById(int id);
        public List<T>? GetAll();
        public T? Find(Func<T,bool> expression);
        public List<T>? FindAll(Func<T,bool> expression);
        //
        public void Update(T entity);
        //
        public void Remove(T entity);
        public void Remove(int id);
        //
        public void Add(T entity);
        //
        public void SaveChanges();
        public Task SaveChangesAsync();

    }
}
