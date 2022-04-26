using System.Reflection;

namespace Salky.WebSocket.Infra.Models
{
    public class RouteInfo : RouteMinimalInfo
    {
        public MethodInfo MethodInfo;
        public Type ClassType { get; set; }
        public void Execute(object instance, object[] parammeters)
        {
            MethodInfo.Invoke(instance, parammeters);
        }
    }

}
