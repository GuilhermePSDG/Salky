namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute]
    public class PingRoute : WebSocketRouteBase
    {

        [WsPost]
        public async void Pong()
        {
            await SendBack("Pong", "ping", Method.POST);
        }

    }
}
