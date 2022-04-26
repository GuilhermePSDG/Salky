using Salky.App.Dtos.Auth;
using Salky.WebSocket.Handler;
using Salky.WebSocket.Infra.Socket;

namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute]
    public class UserRoute : WebSocketRouteBase
    {
        public Guid guid = Guid.NewGuid();
        [WsGet]
        public async void GetUser(SalkyWebSocket ws)
        {
            await SendBack(ws, ws.user, "user", Method.GET);
        }

    }
}
