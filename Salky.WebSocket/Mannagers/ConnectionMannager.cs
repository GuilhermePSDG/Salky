using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Salky.WebSocket.Models;

namespace Salky.WebSocket;

public class ConnectionMannager : WebSocketMannager, IPoolMannager
{
    private ConcurrentDictionary<Key, ConnectionMannager> ConnectionsPools = new();
    //
    public string ConnectionPoolId { get; }
    public ConnectionMannager Previus { get; }
    internal const string PoolRootId = "root";
    public ConnectionMannager()
    {
        ConnectionPoolId = PoolRootId;
        Previus = this;
    }
    private ConnectionMannager(ConnectionMannager Previus, string connectionPoolId)
    {
        ConnectionPoolId = connectionPoolId;
        this.Previus = Previus;
    }
    public int RemoveAllFromPool(Key PoolKey)
    {
        if(!ConnectionsPools.Remove(PoolKey, out var removedPool))
            return -1;
        return removedPool.connections.Count;
    }
    public int RemoveOneFromPool(Key PoolKey, Key ClientToRemoveKey)
    {
        var pool = GetPool(PoolKey);
        if(pool != null)
            return pool.TryRemoveSocket(ClientToRemoveKey, out _) ? 1 : 0;
        return -1;
    }
    public bool AddOneInPool(Key PoolKey, Key ClientToAddId)
    {
        var pool = this.GetOrCreatePool(PoolKey);
        if(Root.TryGetSocket(ClientToAddId, out var sock))
        {
            pool.AddOrUpdate(ClientToAddId, sock);
            return true;
        }
        return false;
    }
    public int AddManyInPool(Key PoolKey, Key[] ClientsKey)
    {
        var pool = this.GetOrCreatePool(PoolKey);
        var root = Root;
        int n = 0;
        foreach(var clientKey in ClientsKey)
        {
            if(root.TryGetSocket(clientKey, out var clientSocket))
            {
                pool.AddOrUpdate(clientKey,clientSocket);
                n++;
            }
        }
        return n;
    }
    public async Task<int> SendToAllInPool(Key PoolKey,MessageServer message)
    {
        var pool = this.GetPool(PoolKey);
        if (pool == null) return -1;
        return await pool.SendToAll(message);
    }
    public async Task<bool> SendToOneInPool(Key PoolKey, Key UserId, MessageServer message)
    {
        var pool = this.GetPool(PoolKey);
        if (pool == null) return false;
        return await pool.SendToOne(UserId, message);
    }
    public IEnumerable<KeyValuePair<string,Storage>> GetStorageOfManyInPool(Key PoolId)
    {
        var pool = this.GetPool(PoolId);
        if (pool == null) yield break;
        foreach(var con in pool.connections)
            yield return new KeyValuePair<string, Storage>(con.Key, con.Value.Storage);
    }
    public override bool TryRemoveSocket(Key Key, [NotNullWhen(true)] out SalkyWebSocket? socket)
    {
        if (base.TryRemoveSocket(Key, out socket))
        {
            DeletePoolIfEmpty(this);
            return true;
        }
        return false;
    }

    private void DeletePoolIfEmpty(ConnectionMannager pool)
    {
        if (pool.ConnectionPoolId != PoolRootId && pool.connections.Count == 0 && pool.ConnectionsPools.Count == 0)
            pool.Previus.RemoveAllFromPool(pool.ConnectionPoolId);
    }
    private ConnectionMannager? GetPool(string PoolKey)
    {
        if (PoolKey == this.ConnectionPoolId) return this;
        if (Root.ConnectionsPools.TryGetValue(PoolKey, out var pool)) return pool;
        return null;
    }
    private ConnectionMannager GetOrCreatePool(string PoolKey) => Root.ConnectionsPools.GetOrAdd(PoolKey, (x) => new ConnectionMannager(this, PoolKey));
    private ConnectionMannager Root => this.ConnectionPoolId == PoolRootId ? this : this.Previus.Root;
    public void PrintDeep(string tab = "")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{tab}> Id : {this.ConnectionPoolId} - Connections : {this.connections.Count} - Childrens Mannagers : {this.ConnectionsPools.Count}");
        tab += "\t";
        foreach (var connM in this.ConnectionsPools.Values)
            connM.PrintDeep(tab);
        Console.ForegroundColor = ConsoleColor.White;
    }

}
