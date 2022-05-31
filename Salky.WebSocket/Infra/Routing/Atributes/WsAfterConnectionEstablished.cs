using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsAfterConnectionEstablished : RouteMethodAtribute
    {
        public WsAfterConnectionEstablished() : this(""){}
        public WsAfterConnectionEstablished(string routePath) : base(routePath, Method._AFTERCONNECTIONOPEN){}
    }

}
