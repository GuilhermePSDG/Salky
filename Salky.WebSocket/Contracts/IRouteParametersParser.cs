using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Interfaces
{
    public interface IRouteParametersParser
    {
        public object[] Parse(RouteInfo route, MessageServer messageServer);
    }

}
