using Salky.App.Storage;

namespace Salky.WebSockets.Implementations
{
    public class LocalStorage : IStorage
    {
        protected virtual IDictionary<string, object> _storage { get; set; } = new Dictionary<string, object>();

        public void Add(string key, object value) => _storage.Add(key, value);

        public void Clear() => _storage.Clear();

        public bool ContainsKey(string key) => _storage.ContainsKey(key);

        public object? Get(string key)
        {
            _storage.TryGetValue(key, out var value);
            return value;
        }
        public bool Remove(string key) => _storage.Remove(key);
        public void AddOrUpdate(string key, object value) => _storage[key] = value;
    }
}
