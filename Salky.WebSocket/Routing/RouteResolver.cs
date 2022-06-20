using System.Reflection;
using Salky.WebSocket.Infra.Socket;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSocket.Extensions;
using Salky.WebSocket.Infra.RoutingExceptions;
using Microsoft.Extensions.Logging;

namespace Salky.WebSocket.Infra.Routing
{
    public class RouteResolver : IRouterResolver
    {
        public static Type[] RouteClassTypes = 
            AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Where(f=> f.GetName().Name == AppDomain.CurrentDomain.FriendlyName)
            .SelectMany(f => f.GetTypes())
            .Where(x => x.GetCustomAttribute(typeof(WebSocketRoute)) != null && x.IsAssignableTo(typeof(WebSocketRouteBase)))
            .ToArray();
        private static Dictionary<string, RouteInfo> Routes = MapRoutes();
        private static MethodInfo WebSocketRouteBase_Inject_Method = typeof(WebSocketRouteBase).GetMethod("Inject", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new NullReferenceException();
        private static MethodInfo WebSocketRouteBase_Constructor_Method = typeof(WebSocketRouteBase).GetMethod("Constructor", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new NullReferenceException();
        
        public ILogger<RouteResolver> Logger { get; }

        public RouteResolver(ILogger<RouteResolver> logger)
        {
            Logger = logger;
        }
       
        public async Task Route(SalkyWebSocket connectionWs, MessageServer messageServer,IServiceProvider provider)
        {
            var key = CreateRouteKey(messageServer.Path, messageServer.Method);
            if (!Routes.TryGetValue(key, out var routeInfoFound) || messageServer.Method.IsInternalMethod())
            {
                await connectionWs.SendErrorAsync($"Cannot resolve route for path : {messageServer.Path} and method : {messageServer.Method}","error");
                return;
            }
            var parameter = ParameterCaster(routeInfoFound, messageServer.Data);
            await ExecuteRoute(connectionWs, routeInfoFound, parameter, provider);
        }
        
        public async Task AfterClose(SalkyWebSocket connectionWs,IServiceProvider provider) => await RouteInternal(connectionWs, Method._CONNECTIONCLOSED,provider);
        public async Task AfterOpen(SalkyWebSocket connectionWs,IServiceProvider provider) => await RouteInternal(connectionWs, Method._AFTERCONNECTIONOPEN,provider);
        private async Task RouteInternal(SalkyWebSocket connectionWs, Method method,IServiceProvider provider)
        {
            if (!method.IsInternalMethod()) throw new InvalidOperationException("Ilegal method.");

            foreach (var route in Routes.Values.Where(x => x.RoutePath.Method == method))
            {
                await ExecuteRoute(connectionWs, route, new object[] { }, provider);
            }
        }
        private async Task ExecuteRoute(SalkyWebSocket connectionWs, RouteInfo route, object[] parameters,IServiceProvider provider)
        {
            var objtIstance = provider.GetRequiredService(route.ClassType);
            var instanceCast= (WebSocketRouteBase)objtIstance;
            instanceCast.Constructor(connectionWs,provider);
            instanceCast.Inject(route.RoutePath.Path);
            var returned = route.Execute(objtIstance, parameters);
            await WaitIfIsTask(returned);
        }

        public record RouteDoc(string Path,string Method,string? ParameterType,object? ParameterJson);
        public static List<RouteDoc> RouteDocs() =>
           Routes.Values
                .Where(x => !x.RoutePath.Method.IsInternalMethod())
                .Select(x => new RouteDoc(
                    x.RoutePath.Path,
                    x.RoutePath.Method.ToString(),
                    x.ParameterType?.Name ,
                    x.ParameterType?.TryCreateInstance() 
                    )).ToList();
        private static Dictionary<string, RouteInfo> MapRoutes()
        {
            Dictionary<string, RouteInfo> InRoutes = new();

            foreach (var @class in RouteClassTypes)
            {
                foreach (var method in @class.GetMethodsWithAtribute<RouteMethodAtribute>())
                {
                    var methodParam = ValidateParameters(method.GetParameters());
                    var atribute = method.GetRequiredAtribute<RouteMethodAtribute>();
                    string fullPath = GetRouteFullPath(@class, atribute);
                    var routeInfo = new RouteInfo
                        (
                        methodInfo: method,
                        classType: @class,
                        routePath: new(fullPath, atribute.routeMethod),
                        parameterType: methodParam.Length == 0 ? null : methodParam[0].ParameterType
                        );
                    var key = CreateRouteKey(fullPath, atribute.routeMethod);
                    if (InRoutes.ContainsKey(key))
                    {
                        throw new DuplicatedRouteKeyException($"Cannot Add RoutePath : {fullPath} Method : {atribute.routeMethod.ToString().ToUpper()} - Reason : Duplicated Route");
                    }
                    InRoutes.Add(key, routeInfo);
                }
            }
            return InRoutes;
        }


        private static string CleanPath(string path) => path.Trim(' ', '/').ToLower();
        private static string CreateRouteKey(string path, Method method) => $"{CleanPath(path)}{method}".ToLower();

        private static ParameterInfo[] ValidateParameters(ParameterInfo[] methodParam)
            =>  methodParam.Length > 1 ?  throw new InvalidRouteException($"A {nameof(WebSocketRoute)} cannot receive more than one parammeter") : methodParam;

        private static string GetRouteFullPath(Type @class, RouteMethodAtribute routeMethodAtribute)
        {
            var wsRouteAtribute = @class.GetRequiredAtribute<WebSocketRoute>();
            string classPath =
                wsRouteAtribute.routeName == null ?
                @class.Name.ToLower().Split("route")[0] : 
                wsRouteAtribute.routeName;
            var methodPath = routeMethodAtribute.routePath;
            var routeFullPath= $"{classPath}/{methodPath}";
            return CleanPath(routeFullPath);
        }


        private object[] ParameterCaster(RouteInfo routeInfo, object? objt)
        {
            if (routeInfo.ParameterType == null) return new object[0];


            switch (objt)
            {
                case JsonElement jsonElement:
                    return new object[] 
                    {
                        JsonSerializer.Deserialize
                        (jsonElement, routeInfo.ParameterType, DefaultJsonSerializerOptions) 
                        ??
                        throw new InvalidRouteParammeterException($"Cannot serialize {jsonElement} into {routeInfo.ParameterType.Name}")
                    };
                case string json:
                    return new object[] 
                    { 
                        JsonSerializer.Deserialize
                        (json, routeInfo.ParameterType, DefaultJsonSerializerOptions
                        ) ??
                        throw new InvalidRouteParammeterException($"Cannot serialize {json} into {routeInfo.ParameterType.Name}")
                    };
                default:
                    throw new InvalidRouteParammeterException("Unorganized route parammeter.");
            }

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
