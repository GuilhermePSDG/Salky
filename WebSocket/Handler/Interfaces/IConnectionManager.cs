using WebSocket.Shared.Models;

namespace WebSocket.Handler.Interfaces
{
    public interface IConnectionManager
    {
        public SalkyWebSocketServer this[string SalkyWebSocketID] { get; set; }
        public void ForEach(Action<SalkyWebSocketServer> sockt);
        public SalkyWebSocketServer Add(SalkyWebSocketServer salkyWebSocket);
        public SalkyWebSocketServer? FindBySockId(string SalkyWebSocketID);
        public SalkyWebSocketServer? FindByUniqueName(string uniqueName);
        public bool Any(string uniqueName);
        public bool TryRemove(string key, out SalkyWebSocketServer? socket);
        public List<UserServer> GetAllVisible();
    }
}