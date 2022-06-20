using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.Models
{
    public class Storage : IDisposable
    {
        internal virtual IDictionary<string, object> _storage { get; set; } = new Dictionary<string,object>();
        #region Get
        public bool TryGet(Type type,out object? value) => _storage.TryGetValue(GetTypeName(type),out value);
        public bool TryGet<T>([NotNullWhen(true)]out T? value) 
        {
            var ok = this._storage.TryGetValue(GetTypeName<T>(), out var retrived);
            value = (ok && retrived != null) ? (T)retrived : default(T);
            return ok;
        }
        public T Get<T>() => (T)this._storage[GetTypeName<T>()];
        public object Get(string key) => this._storage[key];
        public bool TryGet(string key, [NotNullWhen(true)] out object? value) => _storage.TryGetValue(key, out value);
        public T Get<T>(string key) => (T)this._storage[key];
        public object Get(Type type) => _storage[GetTypeName(type)];
        public bool TryGet<T>(string key, [NotNullWhen(true)] out T? value)
        {
            var ok = _storage.TryGetValue(key, out var objt);
            if (ok && objt != null) value = (T)objt;
            else value = default(T);
            return ok;
        }
        public List<T> GetAll<T>()
        {
            var typeT = typeof(T);
            return this._storage
                .Where(x => x.Value.GetType().Equals(typeT)).Select(x => (T)x.Value)
                .ToList();
        }
        public object GetOrCreate(string key,object defaultValue)
        {
            if(this.TryGet(key,out var result))
            {
                return result;
            }
            else
            {
                this.Add(key, defaultValue);
                return this.Get(key);
            }
        }
        public T GetOrCreate<T>(string key, T defaultValue) where T : notnull => (T)this.GetOrCreate(key,defaultValue);
        public T GetOrCreate<T>(string key, Func<T> defaultValueFactory) where T : notnull
        {
            if (this.TryGet<T>(key, out var result))
            {
                return result;
            }
            else
            {
                this.Add(key, defaultValueFactory());
                return this.Get<T>(key);
            }
        }

        #endregion
        #region Add
        public void Add<T>(T data) where T : notnull   => _storage.Add(GetTypeName<T>(), data);
        public void Add(Type type,object data) => _storage.Add(GetTypeName(type), data);
        public void Add(string key, object data) => _storage.Add(key, data);
        public void AddOrUpdate(string key, object data) => _storage[key] = data;
        public void AddOrUpdate<T>(T data) where T : notnull  => _storage[GetTypeName<T>()] = data;
        public void AddOrUpdate(Type type, object data) => _storage[GetTypeName(type)] = data;
        #endregion
      
        public bool TryRemove(string key, [NotNullWhen(true)]out object? value)
        {
            if (this.TryGet(key,out var objt))
            {
                value = objt;
                return this._storage.Remove(key);
            }
            else
            {
                value = null;
                return false;
            }
        }
        
        public bool Has<T>() => _storage.ContainsKey(GetTypeName(typeof(T)));
        public bool Has(Type type) => _storage.ContainsKey(GetTypeName(type));
        public bool Has(string key) => _storage.ContainsKey(key);
        
        
        
        public void ClearStorage() => this._storage.Clear();
        private string GetTypeName<T>() => GetTypeName(typeof(T));
        private string GetTypeName(Type type) => type.FullName ?? throw new NullReferenceException($"Unable to get {nameof(Type.FullName)} of {type.Name}");
        public void Dispose()
        {
            this._storage.Clear();
            this._storage = null;
        }
    }
}
