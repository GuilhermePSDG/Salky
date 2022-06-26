using System.Reflection;

namespace Salky.WebSocket.Infra.Models
{
    public struct RouteInfo 
    {
        public RouteInfo(MethodInfo methodInfo, Type classType, ParameterInfo[] parameterType, RoutePath routePath)
        {
            MethodInfo = methodInfo;
            ClassType = classType;
            Parameters = parameterType;
            RoutePath = routePath;
        }
        
        public MethodInfo MethodInfo { get; }
        public Type ClassType { get; }
        public ParameterInfo[] Parameters { get; }
        public RoutePath RoutePath { get;}

        public object? Execute(object instance, object[] parammeters)
        {
            return MethodInfo.Invoke(instance, parammeters);
        }
    }

}
