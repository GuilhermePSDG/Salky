using Salky.WebSocket.Infra.Socket;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSocket.Extensions;
using Microsoft.Extensions.Logging;
using Salky.WebSocket.Interfaces;

namespace Salky.WebSocket.Infra.Routing
{
    public class RouteResolver : IRouterResolver
    {
        public ILogger<RouteResolver> logger { get; }
        public IRouteList routeList { get; }
        public IRouteParametersParser ParameterParser { get; }
        
        public RouteResolver(ILogger<RouteResolver> logger, IRouteList routeFinder,IRouteParametersParser parameterParser)
        {
            this.logger = logger;
            this.routeList = routeFinder;
            ParameterParser = parameterParser;
        }
        public async Task Route(SalkyWebSocket connectionWs, MessageServer msg,IServiceProvider provider)
        {
            var found = routeList.Find(msg.GetKey());
            if (!found.HasValue)
            {
                await connectionWs.SendErrorAsync($"Cannot resolve route for path : {msg.Path} and method : {msg.Method}","error");
                return;
            }
            var route = found.Value;
            var parameters = this.ParameterParser.Parse(route, msg);
            await ExecuteRoute(connectionWs, route, parameters, provider);
        }

        public async Task AfterClose(SalkyWebSocket connectionWs,IServiceProvider provider) => await ForEachWebSocketRouteBase(provider, connectionWs, (route) => route.OnDisconnectAsync());
        public async Task AfterOpen(SalkyWebSocket connectionWs,IServiceProvider provider) => await ForEachWebSocketRouteBase(provider, connectionWs, (route) =>  route.OnConnectAsync());
        private async Task ForEachWebSocketRouteBase(IServiceProvider provider,SalkyWebSocket ws,Func<WebSocketRouteBase,Task> act)
        {
            foreach (var route in routeList.GetAll())
            {
                await act((WebSocketRouteBase)GetRouteInstance(ws, route, provider));
            }
        }

        private async Task ExecuteRoute(SalkyWebSocket connectionWs, RouteInfo route, object[] parameters,IServiceProvider provider)
        {
            object objtIstance = GetRouteInstance(connectionWs, route, provider);
            var returned = route.Execute(objtIstance, parameters);
            await WaitIfIsTask(returned);
        }

        private static object GetRouteInstance(SalkyWebSocket connectionWs, RouteInfo route, IServiceProvider provider)
        {
            var objtIstance = provider.GetRequiredService(route.ClassType);
            var instanceCast = (WebSocketRouteBase)objtIstance;
            instanceCast.Constructor(connectionWs, provider);
            instanceCast.Inject(route.RoutePath);
            return objtIstance;
        }

        private async Task WaitIfIsTask(object? value)
        {
            if (value != null)
            {
                var returnType = value.GetType();
                if (returnType.BaseType != null && returnType.BaseType.Equals(typeof(Task)))
                {
                    await (Task)value;
                }
            }
        }


    }
}
