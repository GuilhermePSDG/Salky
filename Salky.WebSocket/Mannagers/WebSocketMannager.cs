using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Salky.WebSocket.Models;
using Salky.WebSocket.Exceptions;

namespace Salky.WebSocket;

public class WebSocketMannager : IWebSocketMannager
{
    public ConcurrentDictionary<Key, SalkyWebSocket> connections = new();
    public bool ContainsSocketKey(Key key) => connections.ContainsKey(key);
    public void AddSocket(Key id, SalkyWebSocket salkyWebSocket)
    {
        if (!connections.TryAdd(id, salkyWebSocket))
            throw new DuplicatedConnectionKeyException($"{nameof(SalkyWebSocket)} is already connected, remove before add.");
    }
    public void AddOrUpdate(Key id, SalkyWebSocket salkyWebSocket)
    {
        connections[id] = salkyWebSocket;
    }
    public virtual bool TryRemoveSocket(Key Key, [NotNullWhen(true)] out SalkyWebSocket? socket) => connections.TryRemove(Key, out socket);
    public bool TryGetSocket(Key UserGuidId, [NotNullWhen(true)] out SalkyWebSocket? socket) => connections.TryGetValue(UserGuidId, out socket);
    public async Task<int> SendToAll(MessageServer message)
    {
        int n = 0;
        var tasks = new List<Task>(this.connections.Count);
        foreach (var x in this.connections.Values)
        {
            if (x.ConnectionsIsOpen)
            {
                tasks.Add(x.SendMessageServer(message));
                n++;
            }
        }
        await Task.WhenAll(tasks);
        return n;
    }
    public async Task<bool> SendToOne(Key UserId, MessageServer message) 
    {
        if(this.connections.TryGetValue(UserId,out var sock))
        {
            await sock.SendMessageServer(message);
            return true;
        }
        return false;
    }
}
