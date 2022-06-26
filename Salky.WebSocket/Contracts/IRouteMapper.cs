using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Contracts
{
    public interface IRouteMapper
    {
        public List<RouteInfo> Map();
    }
}
