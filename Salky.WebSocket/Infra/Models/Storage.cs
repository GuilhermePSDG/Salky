using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Models
{
    public class Storage : IDisposable
    {
        private Dictionary<string, object> _storage { get; set; } = new Dictionary<string, object>();

        public bool TryGet(Type type,out object? value) 
        {
            return _storage.TryGetValue(type.Name,out value);
        }
        public bool TryGet<T>(out T? value) where T : class
        {
            var ok = this._storage.TryGetValue(typeof(T).Name, out var retrived);
            if (ok)
                value = (T)retrived;
            else
                value = null;
            return ok;
        }
        public T Get<T>() where T : class
        {
            return (T)this._storage[typeof(T).Name];
        }
        public object Get(Type type) 
        {
            return _storage[type.Name];
        }
        public void Add<T>(T data) where T : class
        {
            _storage.Add(typeof(T).Name, data);
        }

        public void Add(Type type,object data)
        {
            _storage.Add(type.Name, data);
        }

        public void Dispose()
        {
            this._storage.Clear();
            this._storage = null;
        }
    }
}
