using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Salky.WebSocket;


public class RootConnectionMannager : WebSocketMannager
{
    public ConcurrentDictionary<string, SalkyWebSocket> connections = new();
    public bool Contains(string key) => connections.ContainsKey(key);
   
    public void Add(string id, SalkyWebSocket salkyWebSocket)
    {
        if (!connections.TryAdd(id, salkyWebSocket))
            throw new InvalidOperationException($"{nameof(SalkyWebSocket)} is already connected, remove before add.");
    }

    public void AddOrUpdate(string id, SalkyWebSocket salkyWebSocket)
    {
        connections[id] = salkyWebSocket;
    }

    public virtual bool TryRemove(string Key, [NotNullWhen(true)] out SalkyWebSocket? socket) => connections.TryRemove(Key, out socket);
    public bool TryGet(string UserGuidId, [NotNullWhen(true)] out SalkyWebSocket? socket) => connections.TryGetValue(UserGuidId, out socket);
    public async Task SendToAll<T>(string path, Method method, T data, Status status = Status.Success)
    {
        var msg = new MessageServer(path, method, status, data);
        var tasks = new List<Task>(this.connections.Count);
        foreach (var x in this.connections.Values)
            if (x.ConnectionsIsOpen)
                tasks.Add(x.SendMessageServer(msg));
        await Task.WhenAll(tasks);
    }
    public async Task SendToOne<T>(string UserId,string path, Method method, T data, Status status = Status.Success) 
    {
        if(this.connections.TryGetValue(UserId,out var sock))
            await sock.SendMessageServer(new MessageServer(path, method, status, data));
    }
}

public class ConnectionMannager : RootConnectionMannager, IPoolMannager
{
    internal const string PoolRootId = "root";
    public ConnectionMannager(ILogger<ConnectionMannager> logger)
    {
        ConnectionPoolId = PoolRootId;
        Previus = this;
        this.logger = logger;
    }
    private ConnectionMannager(ConnectionMannager Previus, string connectionPoolId, ILogger<ConnectionMannager> logger)
    {
        ConnectionPoolId = connectionPoolId;
        this.Previus = Previus;
        this.logger = logger;
    }
    public override bool TryRemove(string Key, [NotNullWhen(true)] out SalkyWebSocket? socket)
    {
        if(base.TryRemove(Key, out socket))
        {
            DeletePoolIfEmpty(this);
            return true;
        }
        return false;
    }
    
    #region Private Method
    private void DeletePoolIfEmpty(ConnectionMannager pool)
    {
        if (pool.ConnectionPoolId != PoolRootId && pool.connections.Count == 0 && pool.ConnectionsPools.Count == 0)
            pool.Previus.RemoveAllFromPool(pool.ConnectionPoolId);
    }
    private ConnectionMannager? GetPool(string PoolKey)
    {
        if (PoolKey == this.ConnectionPoolId) return this;
        if (Root().ConnectionsPools.TryGetValue(PoolKey, out var pool)) return pool;
        return null;
    }
    private ConnectionMannager? GetOrCreatePool(string PoolKey)
    {
        var root = Root();
        if (PoolKey == this.ConnectionPoolId) return this;
        if (root.ConnectionsPools.TryGetValue(PoolKey, out var pool)) return pool;
        return root.ConnectionsPools.GetOrAdd(PoolKey, new ConnectionMannager(this,PoolKey,this.logger));
    }
    private ConnectionMannager Root() => this.ConnectionPoolId == PoolRootId ? this : this.Previus.Root();
    public void PrintDeep(string tab = "")
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{tab}> Id : {this.ConnectionPoolId} - Connections : {this.connections.Count} - Childrens Mannagers : {this.ConnectionsPools.Count}");
        tab += "    ";
        foreach (var connM in this.ConnectionsPools.Values)
            connM.PrintDeep(tab);
        Console.ForegroundColor = ConsoleColor.White;
    }
    #endregion
    #region Properties
    private ConcurrentDictionary<string, ConnectionMannager> ConnectionsPools = new();
    //
    private readonly ILogger<ConnectionMannager> logger;
    public string ConnectionPoolId { get; }
    public ConnectionMannager Previus { get; }
    #endregion
    #region Functions
    public void RemoveAllFromPool(string connectionPoolId) => ConnectionsPools.Remove(connectionPoolId, out var removedPool);
    public void RemoveFromConnectionPool(string connectionPoolId, string connectionId)
    {
        var pool = GetPool(connectionPoolId);
        if(pool != null) pool.TryRemove(connectionId, out _);
    }
    public void AddInPool(string Poolid, params string[] SocketsId)
    {
        var pool = this.GetOrCreatePool(Poolid);
        var root = Root();
        if (pool != null)
        {
            foreach(var id in SocketsId)
                if(root.TryGet(id, out var sock))
                    pool.AddOrUpdate(id, sock);
        }
    }
    public async Task SendToAllInPool<T>(string PoolId,string path, Method method, T data, Status status = Status.Success) where T : notnull
    {
        var pool = this.GetPool(PoolId);
        if (pool == null) return;
        await pool.SendToAll(path, method, data, status);
    }
    public async Task SendToOneInPool<T>(string PoolId, string UserId, string Path, Method Method, T Data, Status Status = Status.Success) where T : notnull
    {
        var pool = this.GetPool(PoolId);
        if (pool == null) return;
        await pool.SendToOne(UserId,Path, Method, Data, Status);
    }

    public IEnumerable<KeyValuePair<string,Storage>> GetStorageOfManyInPool(string PoolId)
    {
        var pool = this.GetPool(PoolId);
        if (pool == null) yield break;
        foreach(var con in pool.connections)
            yield return new KeyValuePair<string, Storage>(con.Key, con.Value.Storage);
    }
    #endregion

}
