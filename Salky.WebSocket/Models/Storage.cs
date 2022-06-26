using System.Diagnostics.CodeAnalysis;


namespace Salky.WebSocket.Infra.Models
{
    public class Storage : IDisposable
    {
        internal virtual IDictionary<string, object> _storage { get; set; } = new Dictionary<string,object>();
        public bool TryGet<T>([NotNullWhen(true)]out T? value) 
        {
            var ok = this._storage.TryGetValue(GetTypeName<T>(), out var retrived);
            value = (ok && retrived != null) ? (T)retrived : default(T);
            return ok;
        }
        public T Get<T>() => (T)_storage[GetTypeName<T>()];
        public void AddOrUpdate<T>(T data) where T : notnull  => _storage[GetTypeName<T>()] = data;
        public void ClearStorage() => this._storage.Clear();
        private string GetTypeName<T>() => GetTypeName(typeof(T));
        private string GetTypeName(Type type) => type.FullName ?? throw new NullReferenceException($"Unable to get {nameof(Type.FullName)} of {type.Name}");
        public void Dispose()
        {
            ClearStorage();
            this._storage = null;
        }
    }
}
