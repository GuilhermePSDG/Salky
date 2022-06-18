using Salky.WebSocket.Infra.Routing;
using Salky.WebSocket.Infra.Routing.Atributes;

namespace RedisWebSocketTesting
{


    [WebSocketRoute("chat")]
    public class PublicRoute : WebSocketRouteBase
    {
        [WsGet]
        public async Task ping()
        {
            await SendBack
                ("pong", CurrentPath, Salky.WebSocket.Infra.Models.Method.CONFIRM);
        }
    }

}
