using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsGet : RouteMethodAtribute
    {
        public WsGet() : this(""){}
        public WsGet(string routePath) : base(routePath, Method.GET){}
    }

}
