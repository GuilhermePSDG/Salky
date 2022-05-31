using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WsPut : RouteMethodAtribute
    {
        public WsPut() : this(""){}
        public WsPut(string routePath) : base(routePath, Method.PUT){}
    }

}
