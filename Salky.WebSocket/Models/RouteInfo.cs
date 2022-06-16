using System.Reflection;

namespace Salky.WebSocket.Infra.Models
{
    public struct RouteInfo 
    {
        public RouteInfo(MethodInfo methodInfo, Type classType, Type? parameterType, RoutePath routePath)
        {
            MethodInfo = methodInfo;
            ClassType = classType;
            ParameterType = parameterType;
            RoutePath = routePath;
        }

        public MethodInfo MethodInfo { get; }
        public Type ClassType { get; }
        public Type? ParameterType { get; }
        public RoutePath RoutePath { get;}
      


        public object? Execute(object instance, object[] parammeters)
        {
            return MethodInfo.Invoke(instance, parammeters);
        }
    }

}
