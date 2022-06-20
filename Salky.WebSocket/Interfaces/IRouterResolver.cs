using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IRouterResolver
    {
        public Task Route(SalkyWebSocket ws, MessageServer messageServer,IServiceProvider provider);
        public Task AfterClose(SalkyWebSocket connectionWs,IServiceProvider provider);
        public Task AfterOpen(SalkyWebSocket connectionWs,IServiceProvider provider);

    }
}
