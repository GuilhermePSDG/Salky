using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.RoutingExcepetions;
using Salky.WebSocket.Infra.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSocket.Infra.RoutingMinimal
{
    public class MinimalRouting : ISalkyRouter
    {
        public Dictionary<Tuple<string, Method>, RouteMinimalInfo> Routes = new();
        public void Route(SalkyWebSocket ws, MessageServer messageServer)
        {
            var key = new Tuple<string, Method>(messageServer.PathString, messageServer.MethodEnum);
            Routes.TryGetValue(key, out var routeInfoFound);
            if (routeInfoFound == null) throw new InvalidRouteException($"Cannot found result for route : {messageServer.PathString} and method : {messageServer.Method}");
            routeInfoFound.Act(ws,messageServer);    
        }
    }
}
