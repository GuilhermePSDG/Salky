namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute("")]
    public class PingRoute : WebSocketRouteBase
    {
        [WsGet("ping")]
        public async void Pong(SalkyWebSocket ws)
        {
            await SendBack(ws, "Pong", "ping", Method.POST);
        }
    }
}
