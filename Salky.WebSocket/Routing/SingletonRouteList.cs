using Salky.WebSocket.Contracts;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Interfaces;
using System.Collections.Concurrent;

namespace Salky.WebSocket.Routing;

public class SingletonRouteList : IRouteList
{
    private static Dictionary<string, RouteInfo> routes;
    public SingletonRouteList(IRouteMapper routeMapper)
    {
        if(routes == null) routes = new(routeMapper.Map().ToDictionary(x => x.RoutePath.GetKey(), x => x));
    }
    public RouteInfo? Find(string Key) => 
        routes.TryGetValue(Key, out var route) ? route : null;
    public IEnumerable<RouteInfo> GetAll() => routes.Values;
}
