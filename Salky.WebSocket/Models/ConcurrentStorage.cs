using System.Collections.Concurrent;

namespace Salky.WebSocket.Infra.Models
{
    public class ConcurrentStorage : Storage 
    {
        internal override IDictionary<string, object> _storage { get; set; } = new ConcurrentDictionary<string,object>();
    }
}
