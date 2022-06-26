using Salky.WebSocket.Infra.Models;

namespace Salky.WebSocket.Interfaces
{
    public interface IRouteList
    {
        public RouteInfo? Find(string Key);
        public IEnumerable<RouteInfo> GetAll();
    }

}
