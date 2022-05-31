using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsListener : RouteMethodAtribute
    {
        public WsListener() : this(""){}
        public WsListener(string routePath) : base(routePath, Method.LISTENER){}
    }

}
