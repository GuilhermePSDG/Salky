using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared.DataAcess.Models;
using Wpf.Core.Models;

namespace Wpf.Core
{
    public interface IViewModelRepository<T> where T : BaseEntityVM
    {
        //
        public T? GetById(int id);
        public List<T>? GetAll();
        public T? Find(Func<T, bool> expression);
        public List<T>? FindAll(Func<T, bool> expression);
        //
        public void Update(T entity);
        //
        public void Remove(T entity);
        public void Remove(int id);
        //
        public void Add(T entity);
        //
        public int SaveChanges();
        public Task<int> SaveChangesAsync();


    }
}
