using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Infra.Routing.Atributes
{
    public class RouteMethodAtribute : Attribute
    {
        public string routePath { get; set; }
        public Method routeMethod { get; set; }

        public RouteMethodAtribute(string routePath, Method routeMethod)
        {
            this.routePath = routePath;
            this.routeMethod = routeMethod;
        }
    }
}
