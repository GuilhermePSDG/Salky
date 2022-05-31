using Microsoft.Extensions.Logging;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;

namespace Salky.WebSocket.Infra;

public class ConnectionMannager : IConnectionManager
{
    internal const string PoolRootId = "root";
    /// <summary>
    /// Only for the root connection pool
    /// </summary>
    /// <param name="logger"></param>
    public ConnectionMannager(ILogger<ConnectionMannager> logger)
    {
        this.ConnectionPoolId = PoolRootId;
        this.Previus = this;
        this.logger = logger;
    }
    /// <summary>
    /// Only for childs pools
    /// </summary>
    /// <param name="logger"></param>
    private ConnectionMannager(IConnectionManager Previus, string connectionPoolId, ILogger<ConnectionMannager> logger)
    {
        this.ConnectionPoolId = connectionPoolId;
        this.Previus = Previus;
        this.logger = logger;
    }

    #region Properties

    //
    private ConcurrentDictionary<string, SalkyWebSocket> connections = new();
    private ConcurrentDictionary<string, IConnectionManager> ConnectionsPools = new();
    //
    private readonly ILogger<ConnectionMannager> logger;
    public int ConnectionsCount => this.connections.Count;
    public int PoolsCount => this.ConnectionsPools.Count;
    public string ConnectionPoolId { get; }
    public IConnectionManager Previus { get;}
    #endregion

    #region Socket Area
    public bool ContainsSocketKey(string key)
    {
        return this.connections.ContainsKey(key);
    }

    public void ForEachSocket(Action<SalkyWebSocket> action)
    {
        foreach (var x in connections.Values)
            action(x);
    }
    public async Task ForEachSocket(Func<SalkyWebSocket,Task> action)
    {
        foreach (var x in connections.Values)
            await action(x);
    }
    public void AddSocket(string id, SalkyWebSocket salkyWebSocket)
    {
        if (this.connections.ContainsKey(id))
            throw new InvalidOperationException("User is already connected, remove before add.");
        connections[id] = salkyWebSocket;
    }
    public bool RemoveSocket(string key, SalkyWebSocket ws)
    {
        if(this.connections.TryGetValue(key, out var wsGeted))
        {
            if (wsGeted.UniqueId == ws.UniqueId)
            {
                if (this.connections.Remove(key, out var removed))
                {
                    DeletePoolIfEmpty(this);
                    return true;
                }
            }

            if(
                wsGeted.State == WebSocketState.Closed || 
                wsGeted.State == WebSocketState.CloseSent || 
                wsGeted.State == WebSocketState.CloseReceived ||
                wsGeted.State == WebSocketState.Aborted
                
                )
            {
                if (this.connections.Remove(key, out var removed))
                {
                    DeletePoolIfEmpty(this);
                    return true;
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
    public async Task SendToAll<T>(string path, Method method, T data) where T : notnull
    {
        var msg = new MessageServer(data, path, method);
        await this.ForEachSocket(async x =>
        {
            if (x.ConnectionsIsOpen)
            {
                try { 
                    await x.SendMessageServer(msg);
                }
                catch { }
            }
        });
    }
    public async Task SendToAll<T>(Func<SalkyWebSocket, bool> CanSendToThis, string path, Method method, T data) where T : notnull
    {
        var msg = new MessageServer(data, path, method);
        await this.ForEachSocket(async x =>
        {
            if (CanSendToThis(x))
            {
                if (x.ConnectionsIsOpen)
                {
                    try
                    {
                        await x.SendMessageServer(msg);
                    }
                    catch { }
                }
            }

        });
    }
    public async Task SendToAll<T>(Func<SalkyWebSocket, Task<bool>> CanSendToThis, string path, Method method, T data) where T : notnull
    {
        var msg = new MessageServer(data, path, method);
        await this.ForEachSocket(async x =>
        {
            if (await CanSendToThis(x))
            {
                if (x.ConnectionsIsOpen)
                {
                    try
                    {
                        await x.SendMessageServer(msg);
                    }
                    catch { }
                }
            }
        });
    }
    public IEnumerable<T> SelectManySocket<T>(Func<SalkyWebSocket, T> selector)
    {
        return this.connections.Select(x => selector(x.Value));
    }
    public IEnumerable<SalkyWebSocket> WhereSocket(Func<SalkyWebSocket,bool> evaluate)
    {
        return this.connections.Where(x => evaluate(x.Value)).Select(x => x.Value);
    }
    #endregion

    #region ConnectionsPool Area
    public IConnectionManager CreateConnectionPool(string connectionPoolId, params string[] participantsId)
    {
        var ConnectionPoolToAdd = new ConnectionMannager(this,connectionPoolId,this.logger);
        if (!ConnectionsPools.TryAdd(connectionPoolId, ConnectionPoolToAdd))
        {
            throw new InvalidOperationException("Unable to create pool. Pool alredy exist.");
        }
        foreach (var participantId in participantsId)
        {
            var connectionRecovered = this.TryGetSocket(participantId, out var socket);
            if (connectionRecovered && socket != null && socket.ConnectionsIsOpen)
                ConnectionPoolToAdd.AddSocket(participantId, socket);
        }
        return ConnectionPoolToAdd;
    }
    public IConnectionManager GetConnectionPool(string connectionPoolId) => this.ConnectionsPools[connectionPoolId];
    public IConnectionManager GetOrCreateConnectionPool(string PoolId)
    {
        if (this.PoolExist(PoolId))
        {
            return this.ConnectionsPools[PoolId];
        }
        else
        {
            return this.CreateConnectionPool(PoolId);
        }
    }
    public bool TryGetConnectionPool(string connectionPoolId, [NotNullWhen(true)] out IConnectionManager? connectionPool) => ConnectionsPools.TryGetValue(connectionPoolId, out connectionPool);
    public IConnectionManager RemoveConnectionPool(string connectionPoolId)
    {
        if(!this.ConnectionsPools.TryRemove(connectionPoolId, out var removedPool))
            throw new KeyNotFoundException();
        return removedPool;
    }
    public bool TryRemoveConnectionPool(string connectionPoolId, [NotNullWhen(true)] out IConnectionManager? removedPool)
    {
        return this.ConnectionsPools.TryRemove(connectionPoolId, out removedPool);
    }
    public bool PoolExist(string poolId)
    {
        return this.ConnectionsPools.ContainsKey(poolId);
    }
    private void DeletePoolIfEmpty(IConnectionManager pool)
    {
        if (pool.ConnectionPoolId == PoolRootId)
            return;
        if (pool.ConnectionsCount == 0)
        {
            pool.Previus.RemoveConnectionPool(pool.ConnectionPoolId);
        }
    }
    public IConnectionManager? NavigateTo(Func<IConnectionManager, bool> ValidatePool, params string[] PoolsKey)
    {
        IConnectionManager currentPool = this;
        foreach (var path in PoolsKey)
        {
            var ok = currentPool.TryGetConnectionPool(path, out var newPool);
            if (!ok || newPool == null)
                return null;
            if (!ValidatePool(newPool))
                return null;
            currentPool = newPool;
        }
        return currentPool;
    }
    public IConnectionManager? NavigateTo(params string[] PoolsKey)
    {
        IConnectionManager currentPool = this;
        foreach (var path in PoolsKey)
        {
            if (!currentPool.TryGetConnectionPool(path, out var newPool) || newPool == null)
                return null;
            else
                currentPool = newPool;
        }
        return currentPool;
    }
    public async Task<IConnectionManager?> NavigateTo(Func<IConnectionManager, Task<bool>> ValidatePool, params string[] PoolsKey)
    {
        IConnectionManager currentPool = this;
        foreach (var path in PoolsKey)
        {
            var ok = currentPool.TryGetConnectionPool(path, out var newPool);
            if (!ok || newPool == null)
                return null;
            if (!await ValidatePool(newPool))
                return null;
            currentPool = newPool;
        }
        return currentPool;
    }
    #endregion


}
