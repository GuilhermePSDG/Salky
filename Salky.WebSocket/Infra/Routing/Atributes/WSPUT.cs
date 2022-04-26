using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WSPUT : RouteMethodAtribute
    {
        public WSPUT() : this(""){}
        public WSPUT(string routePath) : base(routePath, Method.PUT){}
    }

}
