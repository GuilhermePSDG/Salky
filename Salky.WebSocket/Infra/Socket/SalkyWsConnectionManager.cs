using Salky.WebSocket.Infra.Interfaces;
using System.Collections.Concurrent;

namespace Salky.WebSocket.Infra.Socket;


public class SalkyWsConnectionManager : IConnectionManager
{
    private ConcurrentDictionary<Guid, SalkyWebSocket> _ConnectedSockets = new();
    public SalkyWsConnectionManager() { }

    public SalkyWebSocket this[Guid key] => _ConnectedSockets[key];

    public SalkyWebSocket Add(Guid id,SalkyWebSocket salkyWebSocket)
    {
        var added = _ConnectedSockets.TryAdd(id, salkyWebSocket);
        if (!added) throw new Exception("The socket already exist");
        return salkyWebSocket;
    }

    public bool Any(Func<SalkyWebSocket, bool> expression)
    {
        return _ConnectedSockets.Any(f => expression(f.Value));
    }

    public void ForEach(Action<SalkyWebSocket> action)
    {
        foreach (var x in _ConnectedSockets.Values)
            action(x);
    }

    public SalkyWebSocket? TryGetByKey(Guid UserGuidId)
    {
        _ConnectedSockets.TryGetValue(UserGuidId, out var salkyWebSocket);
        return salkyWebSocket;
    }

    public SalkyWebSocket? TryGetBySocketId(Guid socketId)
    {
        return _ConnectedSockets.SingleOrDefault(x => x.Value.UniqueId.Equals(socketId)).Value;
    }

    public bool TryRemove(Guid key, out SalkyWebSocket? socket)
    {
        return _ConnectedSockets.TryRemove(key, out socket);
    }

    public bool TryRemoveBy(Func<SalkyWebSocket, bool> selector, out SalkyWebSocket? removedSocket)
    {
        KeyValuePair<Guid, SalkyWebSocket>? keyPair = _ConnectedSockets.SingleOrDefault(x => selector(x.Value));
        if (keyPair != null && keyPair.HasValue)
        {
            return _ConnectedSockets.Remove(keyPair.Value.Key, out removedSocket);
        }
        else
        {
            removedSocket = null;
            return false;
        }
    }


}
