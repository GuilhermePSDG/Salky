
namespace WebSocket.Handler.Interfaces
{
    public interface IConnectionManager
    {
        public SalkyWebSocketServer this[byte[] PublicKey] { get; set; }
        public void ForEach(Action<SalkyWebSocketServer> sockt);
        public SalkyWebSocketServer Add(SalkyWebSocketServer salkyWebSocket);
        public SalkyWebSocketServer? FindByUniqueName(string UniqueName);
        public SalkyWebSocketServer? FindByPublicKey(byte[] PublicKey);
        public bool Any(string UniqueName);
        public bool TryRemove(byte[] key, out SalkyWebSocketServer? socket);
        public bool TryRemoveBySocketGUIDID(string id, out SalkyWebSocketServer? socket);
        public List<SalkyWebSocketServer> GetAllVisible();
    }
}