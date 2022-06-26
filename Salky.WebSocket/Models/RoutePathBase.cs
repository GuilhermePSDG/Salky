using System.Text.Json.Serialization;

namespace Salky.WebSocket.Infra.Models
{
    public class RoutePathBase
    {
        public RoutePathBase(string fullPath, Method method)
        {
            this.Path = fullPath;
            this.Method = method;
            this.Key = genKey();
        }
        public RoutePathBase() {}
        private string genKey()
        {
            var r = $"{Path}{Method}".Replace("/", "").Trim().ToLower();
            return r;
        }
        public string GetKey() => Key ??= genKey();
        [NonSerialized,JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        private string Key;
        public string Path { get; init; }
        public Method Method { get; init; }
        public override string ToString() => Key;
    }
}
