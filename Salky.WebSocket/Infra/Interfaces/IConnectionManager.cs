using Salky.WebSocket.Infra.Socket;

namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IConnectionManager
    {
        public SalkyWebSocket this[Guid key] { get; }
        public SalkyWebSocket Add(Guid key,SalkyWebSocket salkyWebSocket);
        public SalkyWebSocket? TryGetByKey(Guid key);
        public bool TryRemove(Guid key, out SalkyWebSocket? removedSocket);
        public bool TryRemoveBy(Func<SalkyWebSocket, bool> selector, out SalkyWebSocket? removedSocket);
        public void ForEach(Action<SalkyWebSocket> action);
        public bool Any(Func<SalkyWebSocket, bool> expression);
    }
}