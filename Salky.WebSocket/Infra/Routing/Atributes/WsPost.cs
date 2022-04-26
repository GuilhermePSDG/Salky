using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsPost : RouteMethodAtribute
    {
        public WsPost() : this(""){}
        public WsPost(string routePath) : base(routePath, Method.POST){}
    }

}
