using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IRouterResolver
    {
        public Task Route(SalkyWebSocket ws, MessageServer messageServer);
        public Task Closed(SalkyWebSocket connectionWs);
        public Task AfterOpen(SalkyWebSocket connectionWs);

    }
}
