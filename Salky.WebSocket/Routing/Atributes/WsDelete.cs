using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsDelete : RouteMethodAtribute
    {
        public WsDelete() : this(""){}
        public WsDelete(string routePath) : base(routePath, Method.DELETE){}
    }

}
