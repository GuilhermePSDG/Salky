global using Salky.WebSocket.Infra.Routing.Atributes;
global using Salky.WebSocket.Infra.Routing;
global using static Salky.WebSocket.Global;
global using System.Text.Json;
using System.Text.Json.Serialization;

namespace Salky.WebSocket
{
    internal static class Global
    {
        public static JsonSerializerOptions DefaultJsonSerializerOptions = CreateOptions();
        
        private static JsonSerializerOptions CreateOptions()
        {
            var opt = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            opt.Converters.Add(new JsonStringEnumConverter());
            return opt;
        }
    }
}
