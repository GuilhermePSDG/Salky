using System.Collections.Concurrent;
using WebSocket.Handler.Interfaces;
using WebSocket.Shared.Models;

namespace WebSocket.Handler;


public class SalkyWebSocketConnectionManager  : IConnectionManager
{
    private ConcurrentDictionary<string, SalkyWebSocketServer> _SocketsDict = new ();
    public int count => _SocketsDict.Count;

    private string ByteToString(byte[] bytes) => Convert.ToBase64String(bytes);
    private byte[] StringToByte(string bytes) => Convert.FromBase64String(bytes);

    public SalkyWebSocketServer this[byte[] PublicKey] 
    {
        get => _SocketsDict[ByteToString(PublicKey)];
        set => _SocketsDict[ByteToString(PublicKey)] = value;
    }
    public void ForEach(Action<SalkyWebSocketServer> sockt)
    {
        foreach (var x in _SocketsDict.Values)
            sockt(x);
    }
    //Oque fazer caso já tenha alguém logado com essa chave public
    //Simplesmente deixar ?
    public SalkyWebSocketServer Add(SalkyWebSocketServer socket)
    {
        var ws = _SocketsDict.GetOrAdd(ByteToString(socket.user.PublicKey), socket);
        return ws;
    }
    public List<SalkyWebSocketServer> GetAllVisible()
    {
        return this._SocketsDict.Where(x => x.Value.user.IsVisible).Select(q => q.Value).ToList();
    }
    public SalkyWebSocketServer? FindByUniqueName(string uniqueName)
    {
        return _SocketsDict.FirstOrDefault(q => q.Value.user.Apelido.Equals(uniqueName)).Value;
    }
    public bool TryRemove(byte[] key, out SalkyWebSocketServer? socket)
    {
        var removed = _SocketsDict.TryRemove(ByteToString(key), out SalkyWebSocketServer? sock);
        socket = sock;
        return removed;
    }

    public bool Any(string uniqueName)
    {
        return _SocketsDict.Any(q => q.Value.user?.Apelido == uniqueName);
    }

    public SalkyWebSocketServer? FindByPublicKey(byte[] PublicKey)
    {
        if (_SocketsDict.ContainsKey(ByteToString(PublicKey)))
            return _SocketsDict[ByteToString(PublicKey)];
        else
            return null;
    }

    public bool TryRemoveBySocketGUIDID(string id, out SalkyWebSocketServer? socket)
    {
        KeyValuePair<string,SalkyWebSocketServer>? socketFound = _SocketsDict.FirstOrDefault(x => x.Value.GUIDID.Equals(id));
        if(socketFound == null || !socketFound.HasValue)
        {
            socket = null;
            return false;
        }
        else
        {
            var removed = _SocketsDict.TryRemove(socketFound.Value.Key, out SalkyWebSocketServer? sock);
            socket = sock;
            return removed;
        }


      
    }
}
