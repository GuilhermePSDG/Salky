using Microsoft.Extensions.Logging;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;

namespace Salky.WebSocket;

public class ConnectionMannager : IConnectionManager
{
    internal const string PoolRootId = "root";
    /// <summary>
    /// Only for the root connection pool
    /// </summary>
    /// <param name="logger"></param>
    public ConnectionMannager(ILogger<ConnectionMannager> logger)
    {
        ConnectionPoolId = PoolRootId;
        Previus = this;
        this.logger = logger;
    }
    /// <summary>
    /// Only for childs pools
    /// </summary>
    /// <param name="logger"></param>
    private ConnectionMannager(IConnectionManager Previus, string connectionPoolId, ILogger<ConnectionMannager> logger)
    {
        ConnectionPoolId = connectionPoolId;
        this.Previus = Previus;
        this.logger = logger;
    }

    #region Properties


    public void PrintDeep(string tab = "")
    {
        Console.WriteLine($"{tab}Id : {this.ConnectionPoolId} - Connections : {this.ConnectionsCount} - Childrens Mannagers : {this.PoolsCount}");
        tab += "    ";
        foreach (var connM in this.ConnectionsPools.Values)
            (connM as ConnectionMannager).PrintDeep(tab);
    }

    //
    private ConcurrentDictionary<string, SalkyWebSocket> connections = new();
    private ConcurrentDictionary<string, IConnectionManager> ConnectionsPools = new();
    //
    private readonly ILogger<ConnectionMannager> logger;
    public int ConnectionsCount => connections.Count;
    public int PoolsCount => ConnectionsPools.Count;
    public string ConnectionPoolId { get; }
    public IConnectionManager Previus { get; }
    #endregion

    #region Socket Area
    public bool ContainsSocketKey(string key) => connections.ContainsKey(key);
    public void ForEachSocket(Action<SalkyWebSocket> action)
    {
        foreach (var x in connections.Values)
            action(x);
    }

    public async Task ForEachSocket(Func<SalkyWebSocket, Task> action)
    {
        var tasks = new List<Task>(this.ConnectionsCount);
        foreach (var x in connections.Values)
            tasks.Add(action(x));
        await Task.WhenAll(tasks);
    }
    public void AddSocket(string id, SalkyWebSocket salkyWebSocket)
    {
        if(!connections.TryAdd(id, salkyWebSocket))
            throw new InvalidOperationException("User is already connected, remove before add.");
    }
    public bool RemoveSocket(string key, SalkyWebSocket ws)
    {
        if (connections.TryGetValue(key, out var wsGeted))
        {
            if (wsGeted.UniqueId == ws.UniqueId)
            {
                if (connections.Remove(key, out var removed))
                {
                    DeletePoolIfEmpty(this);
                    return true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            if (
                wsGeted.State == WebSocketState.Closed ||
                wsGeted.State == WebSocketState.CloseSent ||
                wsGeted.State == WebSocketState.CloseReceived ||
                wsGeted.State == WebSocketState.Aborted

                )
            {
                if (connections.Remove(key, out var removed))
                {
                    DeletePoolIfEmpty(this);
                    return true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        return false;
    }
    public bool TryRemoveSocket(string key, [NotNullWhen(true)] out SalkyWebSocket? socket)
    {
        var removed = connections.TryRemove(key, out socket);
        DeletePoolIfEmpty(this);
        return removed;
    }
    public bool TryGetSocket(string UserGuidId, [NotNullWhen(true)] out SalkyWebSocket? socket) => connections.TryGetValue(UserGuidId, out socket);
    public async Task SendToAll<T>(string path, Method method, T data, Status status = Status.Success) where T : notnull
    {
        var msg = new MessageServer(path, method, status, data);
        var tasks = new List<Task>(this.ConnectionsCount);
        ForEachSocket(x =>
        {
            if (x.ConnectionsIsOpen)
                tasks.Add(x.SendMessageServer(msg));
        });
        await Task.WhenAll(tasks);
    }
    public async Task SendToAll<T>(Func<SalkyWebSocket, bool> CanSendToThis, string path, Method method, T data, Status status = Status.Success) where T : notnull
    {
        var msg = new MessageServer(path, method, status, data);
        var tasks = new List<Task>(this.ConnectionsCount);
        ForEachSocket(x =>
        {
            if (x.ConnectionsIsOpen && CanSendToThis(x)) 
                tasks.Add(x.SendMessageServer(msg));
        });
        await Task.WhenAll(tasks);
    }
    public async Task SendToAll<T>(Func<SalkyWebSocket, Task<bool>> CanSendToThis, string path, Method method, T data, Status status = Status.Success) where T : notnull
    {
        var msg = new MessageServer(path, method, status, data);
        var tasks = new List<Task>(this.ConnectionsCount);
        await ForEachSocket(async x =>
        {
            if (x.ConnectionsIsOpen && await CanSendToThis(x))
                tasks.Add(x.SendMessageServer(msg));
        });
        await Task.WhenAll(tasks);
    }
    public IEnumerable<T> SelectManySocket<T>(Func<SalkyWebSocket, T> selector) => connections.Select(x => selector(x.Value));
    public IEnumerable<SalkyWebSocket> WhereSocket(Func<SalkyWebSocket, bool> evaluate) => connections.Where(x => evaluate(x.Value)).Select(x => x.Value);
    #endregion

    #region ConnectionsPool Area
    public IConnectionManager CreateConnectionPool(string connectionPoolId, params string[] participantsId)
    {
        var ConnectionPoolToAdd = new ConnectionMannager(this, connectionPoolId, logger);
        if (!ConnectionsPools.TryAdd(connectionPoolId, ConnectionPoolToAdd)) 
            throw new InvalidOperationException("Unable to create pool. Pool alredy exist."); 
        foreach (var participantId in participantsId)
        {
            if (TryGetSocket(participantId, out var socket) && 
                socket != null && 
                socket.ConnectionsIsOpen)
                ConnectionPoolToAdd.AddSocket(participantId, socket);
        }
        return ConnectionsPools[connectionPoolId];
    }
    public IConnectionManager GetConnectionPool(string connectionPoolId) => ConnectionsPools[connectionPoolId];
    public IConnectionManager GetOrCreateConnectionPool(string PoolId) => PoolExist(PoolId) ? ConnectionsPools[PoolId] : CreateConnectionPool(PoolId);
    public bool TryGetConnectionPool(string connectionPoolId, [NotNullWhen(true)] out IConnectionManager? connectionPool) => ConnectionsPools.TryGetValue(connectionPoolId, out connectionPool);
    public IConnectionManager RemoveConnectionPool(string connectionPoolId) => ConnectionsPools.Remove(connectionPoolId, out var removedPool) ? removedPool : throw new KeyNotFoundException();
    public bool TryRemoveConnectionPool(string connectionPoolId, [NotNullWhen(true)] out IConnectionManager? removedPool) => ConnectionsPools.TryRemove(connectionPoolId, out removedPool);
    public bool PoolExist(string poolId) => ConnectionsPools.ContainsKey(poolId);
    private void DeletePoolIfEmpty(IConnectionManager pool)
    {
        if (pool.ConnectionPoolId != PoolRootId && pool.ConnectionsCount == 0)
            pool.Previus.RemoveConnectionPool(pool.ConnectionPoolId);
    }
    public IConnectionManager? NavigateTo(Func<IConnectionManager, bool> ValidatePool, params string[] PoolsKey)
    {
        IConnectionManager? currentPool = this;
        foreach (var path in PoolsKey)
            if (!currentPool.TryGetConnectionPool(path, out currentPool) || !ValidatePool(currentPool))
                return null;
        return currentPool;
    }
    public IConnectionManager? NavigateTo(params string[] PoolsKey)
    {
        IConnectionManager? currentPool = this;
        foreach (var path in PoolsKey)
            if (!currentPool.TryGetConnectionPool(path, out currentPool))
                return null;
        return currentPool;
    }
    public async Task<IConnectionManager?> NavigateTo(Func<IConnectionManager, Task<bool>> ValidatePool, params string[] PoolsKey)
    {
        IConnectionManager? currentPool = this;
        foreach (var path in PoolsKey)
            if (!currentPool.TryGetConnectionPool(path, out currentPool) || !await ValidatePool(currentPool))
                return null;
        return currentPool;
    }
    public IConnectionManager BackToRoot() => Previus.ConnectionPoolId == PoolRootId ? Previus : Previus.BackToRoot();
    #endregion

}
