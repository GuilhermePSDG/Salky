using System.Reflection;
using Salky.WebSocket.Infra.Socket;
using Newtonsoft.Json;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSocket.Extensions;
using Salky.WebSocket.Infra.RoutingExcepetions;

namespace Salky.WebSocket.Infra.Routing
{
    public class RouteResolver : ISalkyRouter
    {
        private Dictionary<string, RouteInfo> Routes = new();
        private static Type[]? _RouteClassTypes;
        public static Type[] RouteClassTypes
        {
            get
            {
                if (_RouteClassTypes == null)
                    _RouteClassTypes = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .Where( f=> f.GetName().Name == AppDomain.CurrentDomain.FriendlyName)
                        .SelectMany(f => f.GetTypes())
                        .Where(x => x.GetCustomAttribute(typeof(WebSocketRoute)) != null && x.IsAssignableTo(typeof(WebSocketRouteBase)))
                        .ToArray();
                return _RouteClassTypes;
            }
        }
        public RouteResolver() => InitiateRoutes();
        public void Route(SalkyWebSocket connectionWs, MessageServer messageServer)
        {
            var key = CreateKey(messageServer.PathString, messageServer.MethodEnum);
            var ok = Routes.TryGetValue(key,out var routeInfoFound);
            if (!ok) throw new InvalidRouteException($"Cannot resolve route for path : {messageServer.PathString} and method : {messageServer.Method}");
            var objtIstance = RecoveryInstance(connectionWs,routeInfoFound.ClassType);
            var parameter = ParameterCaster(routeInfoFound.MethodInfo, connectionWs, messageServer);
            routeInfoFound.Execute(objtIstance, parameter);
        }
        private static string CreateKey(string fullPath, Method method) => $"{fullPath}-{method}".ToLower();
        
        private object RecoveryInstance(SalkyWebSocket connectionWs, Type type)
        {
            object instance;
            if (connectionWs.Storage.TryGet(type, out var instaceRecovred))
            {
                instance = instaceRecovred ?? 
                    throw new NullReferenceException($"Cannot recovery a instance of {type.FullName} from {nameof(SalkyWebSocket.Storage)}");
            }
            else
            {
                instance = connectionWs.Storage
                    .Get<IServiceProvider>()
                    .GetRequiredService(type);
                connectionWs.Storage.Add(type, instance);
            }
            return instance;
        }

        private void InitiateRoutes()
        {
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
                        Method = atribute.routeMethod,
                        Path = fullPath,
                    };
                    var key = CreateKey(fullPath, atribute.routeMethod);
                    Routes.Add(key, routeInfo);
                }
            }
        }


        private static void ValidateParameters(ParameterInfo[] methodParam)
        {


        }

        private static string GetRouteFullPath(Type @class, RouteMethodAtribute routeMethodAtribute)
        {
            var wsRouteAtribute = @class.GetRequiredAtribute<WebSocketRoute>();
            string routeBasePath =
                wsRouteAtribute.routeName == null ?
                @class.Name.ToLower().Split("route")[0] : 
                wsRouteAtribute.routeName;
          
            var methodPath = routeMethodAtribute.routePath.ToLower();

            var routeFullPath= $"{routeBasePath}/{methodPath}".Trim('/');
            return routeFullPath;
        }

        private static object[] ParameterCaster(MethodInfo methodInfo,SalkyWebSocket salkyWebSocket,MessageServer msg)
        {
            var parameters = methodInfo.GetParameters();
            var parametersResult = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.Equals(typeof(SalkyWebSocket)))
                    parametersResult[i] = salkyWebSocket;
                else if (parameters[i].ParameterType.Equals(typeof(MessageServer))) 
                    parametersResult[i] = msg;
                else
                {

                    try
                    {
                        var result = JsonConvert.DeserializeObject(msg.DataJson, parameters[i].ParameterType);
                        if (result == null)  throw new Exception();
                        parametersResult[i] = result;
                    }
                    catch
                    {
                        var objectTemplate = parameters[i].ParameterType.TryCreateInstance();
                        var jsonTemplate = JsonConvert.SerializeObject(objectTemplate, Formatting.Indented);
                        throw new InvalidRouteParammeterException($"Cannot serialize {msg.DataJson} into {jsonTemplate}");
                    }
                }
            }
            return parametersResult;
        }

    }

}
