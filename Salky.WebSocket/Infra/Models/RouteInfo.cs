using System.Reflection;

namespace Salky.WebSocket.Infra.Models
{
    public class RouteInfo 
    {
        public MethodInfo MethodInfo;
        public Type ClassType { get; set; }
        public Type? ParameterType { get; set; }
        public RoutePath RoutePath { get; set; }

        public object? Execute(object instance, object[] parammeters)
        {
            return MethodInfo.Invoke(instance, parammeters);
        }
    }

}
