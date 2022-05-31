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
        private static MethodInfo WebSocketRouteBase_Inject_Method = typeof(WebSocketRouteBase).GetMethod("Inject", BindingFlags.NonPublic | BindingFlags.Instance);

        public ILogger<RouteResolver> Logger { get; }

        public RouteResolver(ILogger<RouteResolver> logger)
        {
            Logger = logger;
        }

       
        public async Task Route(SalkyWebSocket connectionWs, MessageServer messageServer)
        {
            try
            {
                var key = CreateRouteKey(messageServer.Path, messageServer.Method);
                if (!Routes.TryGetValue(key, out var routeInfoFound) || (int)messageServer.Method < 0)
                {
                    await connectionWs.SendErrorAsync($"Cannot resolve route for path : {messageServer.Path} and method : {messageServer.Method}");
                    return;
                }
                var objtIstance = RecoveryObjectInstance(connectionWs, routeInfoFound.ClassType);
                var parameter = ParameterCaster(routeInfoFound, messageServer.Data);
                WebSocketRouteBase_Inject_Method.Invoke(objtIstance, new object[] { connectionWs,CleanPath(messageServer.Path) });
                var returned = routeInfoFound.Execute(objtIstance, parameter);
                await WaitIfIsTask(returned);
            }
            catch(Exception ex)
            {
                this.Logger.LogError(ex, "Erro no roteamento");
            }
        }

        public async Task Closed(SalkyWebSocket connectionWs)
        {
            foreach (var route in Routes)
            {
                if(route.Value.RoutePath.Method == Method._CONNECTIONCLOSED)
                {
                    var objtIstance = RecoveryObjectInstance(connectionWs, route.Value.ClassType);
                    WebSocketRouteBase_Inject_Method.Invoke(objtIstance, new object[] { connectionWs ,""});
                    var returned = route.Value.Execute(objtIstance, new object[] {});
                    await WaitIfIsTask(returned);
                }
            }
        }
        public async Task AfterOpen(SalkyWebSocket connectionWs)
        {
            foreach (var route in Routes)
            {
                if (route.Value.RoutePath.Method == Method._AFTERCONNECTIONOPEN)
                {
                    var objtIstance = RecoveryObjectInstance(connectionWs, route.Value.ClassType);
                    WebSocketRouteBase_Inject_Method.Invoke(objtIstance, new object[] { connectionWs ,""});
                    var returned = route.Value.Execute(objtIstance, new object[] { });
                    await WaitIfIsTask(returned);
                }
            }
        }

        private static Dictionary<string, RouteInfo> MapRoutes()
        {
            Dictionary<string, RouteInfo> InRoutes = new();

            foreach (var @class in RouteClassTypes)
            {
                var routeMethods = @class.GetMethodsWithAtribute<RouteMethodAtribute>();
                foreach (var method in routeMethods)
                {
                    var methodParam = method.GetParameters();
                    ValidateParameters(methodParam);
                    var atribute = method.GetRequiredAtribute<RouteMethodAtribute>();
                    string fullPath = GetRouteFullPath(@class, atribute);
                    var routeInfo = new RouteInfo()
                    {
                        ClassType = @class,
                        MethodInfo = method,
                        RoutePath = new(fullPath, atribute.routeMethod),
                        ParameterType = methodParam.Length ==  0 ? null : methodParam[0].ParameterType
                    };
                    var key = CreateRouteKey(fullPath, atribute.routeMethod);
                    if (InRoutes.ContainsKey(key))
                        throw new DuplicatedWebSocketRouteExcepetion($"Cannot Add RoutePath : {fullPath} Method : {atribute.routeMethod.ToString().ToUpper()} - Reason : Duplicated Route");
                    InRoutes.Add(key, routeInfo);
                }
            }
            return InRoutes;
        }


        private static string CleanPath(string path)
        {
            return path.Trim(' ', '/').ToLower();
        }
        private static string CreateRouteKey(string path, Method method)
        {
            return $"{CleanPath(path)}{method}".ToLower();
        }
        
        private static object RecoveryObjectInstance(SalkyWebSocket connectionWs, Type type)
        {
            return connectionWs.Storage.Get<IServiceProvider>().GetRequiredService(type);
        }

        private static void ValidateParameters(ParameterInfo[] methodParam)
        {
            if(methodParam.Length > 1) throw new Exception($"A {nameof(WebSocketRoute)} cannot receive more than one parammeter");
        }

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

        /// <summary>
        /// Return a object[] with just one object
        /// <para></para>
        /// Work only with System.Text.json
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        /// <exception cref="InvalidRouteParammeterException"></exception>
        private static object[] ParameterCaster(RouteInfo routeInfo, object? objt)
        {
            if (routeInfo.ParameterType == null) return new object[0];
            try
            {
                var casted = (JsonElement?)objt;
                if (casted == null) throw new Exception();
                var res = JsonSerializer.Deserialize(casted.Value, routeInfo.ParameterType,DefaultJsonSerializerOptions);
                if (res == null) throw new Exception();
                return new object[] { res};
            }
            catch
            {
                var error_msg = $"Cannot serialize Data into {routeInfo.ParameterType.Name}";
                var objectTemplate = routeInfo.ParameterType.TryCreateInstance();
                if (objectTemplate != null)
                {
                    var jsonTemplate = JsonSerializer.Serialize(objectTemplate,DefaultJsonSerializerOptions);
                    error_msg += $"\r\nJson Template : {jsonTemplate}";
                }
                throw new InvalidRouteParammeterException(error_msg);
            }
        }

        private static async Task WaitIfIsTask(object? value)
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
