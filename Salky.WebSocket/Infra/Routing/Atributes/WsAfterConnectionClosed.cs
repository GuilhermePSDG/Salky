using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsAfterConnectionClosed : RouteMethodAtribute
    {
        public WsAfterConnectionClosed() : this(""){}
        public WsAfterConnectionClosed(string routePath) : base(routePath, Method._CONNECTIONCLOSED){}
    }

}
