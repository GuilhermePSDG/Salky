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
        /// <summary>
        /// <list type="bullet">Obtem um item baseado no nome do tipo/></list>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns><see cref="bool"/> - true se obteve, false se não</returns>
        public bool TryGet(Type type,out object? value) 
        {
            return _storage.TryGetValue(GetTypeName(type),out value);
        }
        /// <summary>
        /// <list type="bullet">Obtem um item baseado no nome do tipo e o converte em <typeparamref name="T"/></list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns><see cref="bool"/> - true se obteve, false se não</returns>
        public bool TryGet<T>([NotNullWhen(true)]out T? value) 
        {
            var ok = this._storage.TryGetValue(GetTypeName<T>(), out var retrived);
            value = (ok && retrived != null) ? (T)retrived : default(T);
            return ok;
        }
        /// <summary>
        /// <list type="bullet">Obtem um item baseado no nome do tipo e o converte em <typeparamref name="T"/></list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns><typeparamref name="T"/></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public T Get<T>() where T : class
        {
            return (T)this._storage[GetTypeName<T>()];
        }
        /// <summary>
        /// <list type="bullet">Obtem um item baseado na sua chave e o converte em <typeparamref name="T"/></list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns><typeparamref name="T"/></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public T Get<T>(string key)
        {
            return (T)this._storage[key];
        }
        /// <summary>
        /// <list type="bullet">Obtem um item baseado na sua chave</list>
        /// </summary>
        /// <param name="key"></param>
        /// <returns><see cref="object"/> </returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public object Get(string key)
        {
            return this._storage[key];
        }
        public bool TryGet(string key, [NotNullWhen(true)] out object? value)
        {
            return _storage.TryGetValue(key, out value);
        }
        public bool TryGet<T>(string key, [NotNullWhen(true)] out T? value) where T : class
        {
            var ok =  _storage.TryGetValue(key, out var objt);
            if (ok && objt != null) value = (T)objt;
            else value = null;
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
        public T GetOrCreate<T>(string key, T defaultValue) where T : notnull
        {
            if (this.TryGet(key, out var result))
            {
                return (T)result;
            }
            else
            {
                this.Add(key, defaultValue);
                return (T)this.Get(key);
            }
        }
        public object Get(Type type) 
        {
            return _storage[GetTypeName(type)];
        }
        #endregion
        #region Add
        public void Add<T>(T data) where T : class
        {
            _storage.Add(GetTypeName<T>(), data);
        }
      

        public void Add(Type type,object data)
        {
            _storage.Add(GetTypeName(type), data);
        }

        public void Add(string key, object data)
        {
            _storage.Add(key, data);
        }

        public void AddOrUpdate(string key, object data)
        {
            _storage[key] = data;
        }
        public void AddOrUpdate<T>(T data) where T : class
        {
            _storage[GetTypeName<T>()] = data;
        }
        public void AddOrUpdate(Type type, object data)
        {
            _storage[GetTypeName(type)] = data;
        }

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
        
        /// <summary>
        /// Clear all objects in <see cref="Storage"/>
        /// </summary>
        public void ClearStorage() => this._storage.Clear();
        private string GetTypeName<T>()
        {
            var type = typeof(T);
            return GetTypeName(type);
        }
        private string GetTypeName(Type type)
        {
            var name = type.FullName;
            if (name == null) throw new NullReferenceException($"Unable to get {nameof(Type.FullName)} of {type.Name}");
            return name;
        }
        public void Dispose()
        {
            this._storage.Clear();
            this._storage = null;
        }
    }
}
