using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class WSDELETE : RouteMethodAtribute
    {
        public WSDELETE() : this(""){}
        public WSDELETE(string routePath) : base(routePath, Method.DELETE){}
    }

}
