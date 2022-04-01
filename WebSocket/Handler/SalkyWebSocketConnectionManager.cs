using System.Collections.Concurrent;
using WebSocket.Handler.Interfaces;
using WebSocket.Shared.Models;

namespace WebSocket.Handler;


public class SalkyWebSocketConnectionManager  : IConnectionManager
{
    private ConcurrentDictionary<string, SalkyWebSocketServer> _SocketsDict = new ();
    public int count => _SocketsDict.Count;
    public SalkyWebSocketServer this[string SalkyWebSocketID]
    {
        get => _SocketsDict[SalkyWebSocketID];
        set => _SocketsDict[SalkyWebSocketID] = value;
    }
    public void ForEach(Action<SalkyWebSocketServer> sockt)
    {
        foreach (var x in _SocketsDict.Values)
            sockt(x);
    }
    public SalkyWebSocketServer Add(SalkyWebSocketServer socket)
    {
        var ws = new SalkyWebSocketServer(socket);
        ws = _SocketsDict.GetOrAdd(ws.Id, ws);
        return ws;
    }
    public SalkyWebSocketServer? FindBySockId(string SalkyWebSocketID) => _SocketsDict.Where(q => q.Key.Equals(SalkyWebSocketID)).Select(n => n.Value).FirstOrDefault();
    public List<UserServer> GetAllVisible() => this._SocketsDict.Where(x => x.Value.user != null && x.Value.user.IsVisible).Select(q => q.Value.user ?? throw new NullReferenceException()).ToList();
    public SalkyWebSocketServer? FindByUniqueName(string uniqueName) => _SocketsDict.Where(q => q.Value.user != null && q.Value.user.Apelido.Equals(uniqueName)).Select(q => q.Value).FirstOrDefault();
    public bool TryRemove(string key, out SalkyWebSocketServer? socket)
    {
        var removed = _SocketsDict.TryRemove(key, out SalkyWebSocketServer? sock);
        socket = sock;
        return removed;
    }

    public bool Any(string uniqueName)
    {
        return _SocketsDict.Any(q => q.Value.user?.Apelido == uniqueName);
    }
}
