using Salky.WebSocket.Infra.Socket;


namespace Salky.WebSocket.Infra.Models
{
    public class RouteMinimalInfo
    {
        public string Path { get; set; }
        public Method Method { get; set; }
        public Action<SalkyWebSocket,MessageServer> Act;
    }
}
