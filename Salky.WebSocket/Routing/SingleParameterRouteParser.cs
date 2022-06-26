using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.RoutingExceptions;
using Salky.WebSocket.Interfaces;

namespace Salky.WebSocket.Infra.Routing
{
    public class SingleParameterRouteParser : IRouteParametersParser
    {
        public object[] Parse(RouteInfo route, MessageServer messageServer)
        {
            if (route.Parameters == null || route.Parameters.Length == 0) return new object[0];
            var param = route.Parameters[0];
            var paramType = param.ParameterType;
            switch (messageServer.Data)
            {
                case JsonElement jsonElement:
                    return new object[] { JsonElementParser(jsonElement, paramType) };
                case string json:
                    return new object[] { JsonStringParser(json, paramType) };
                default:
                    throw new InvalidRouteParammeterException("Unorganized route parammeter.");
            }
        }
        private object JsonElementParser(JsonElement element,Type targetType)
        {
            return element.Deserialize(targetType, DefaultJsonSerializerOptions) ?? throw new InvalidRouteParammeterException($"Cannot serialize message into {targetType.Name}");
        }
        private object JsonStringParser(string Json,Type targetType)
        {
            return JsonSerializer.Deserialize(Json, targetType, DefaultJsonSerializerOptions) ?? throw new InvalidRouteParammeterException($"Cannot serialize message into {targetType.Name}");
        }
    }
  
}
