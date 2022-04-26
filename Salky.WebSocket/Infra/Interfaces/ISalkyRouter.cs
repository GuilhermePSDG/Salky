using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface ISalkyRouter
    {
        public void Route(SalkyWebSocket ws, MessageServer messageServer);
    }
}
