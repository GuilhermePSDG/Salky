using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;

namespace Salky.WebSocket.Infra.RoutingMinimal
{

    public interface RouteBuilderThen
    {
        public RouteBuilderOn Build();
    }
    public interface RouteBuilderDo
    {
        RouteBuilderThen Do(Action<SalkyWebSocket, MessageServer> handler);
    }
    public interface RouteBuilderOn
    {
        RouteBuilderDo On(string routeName, Method method = Method.GET);
    }

    public class RoutingBuilder : RouteBuilderThen, RouteBuilderDo, RouteBuilderOn
    {
        private RouteMinimalInfo routeInfo = new RouteMinimalInfo();
        private MinimalRouting context;

        private RoutingBuilder(MinimalRouting minimalRouting)
        {
            context = minimalRouting;
        }

        public RouteBuilderOn Build()
        {
            context.Routes.Add(new Tuple<string, Method>(routeInfo.Path, routeInfo.Method), routeInfo);
            routeInfo = new RouteMinimalInfo();
            return this;
        }

        public static RouteBuilderOn Create(MinimalRouting minimalRouting)
            => new RoutingBuilder(minimalRouting);

        public RouteBuilderThen Do(Action<SalkyWebSocket, MessageServer> handler)
        {
            routeInfo.Act = handler;
            return this;
        }

        public RouteBuilderDo On(string routeName, Method method = Method.GET)
        {
            routeInfo.Path = routeName;
            routeInfo.Method = method;
            return this;
        }
    }

}
