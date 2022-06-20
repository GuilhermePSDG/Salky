using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
namespace Salky.WebSocket.Infra.Interfaces
{

    public interface WebSocketMannager : IManager<SalkyWebSocket>
    {
        public Task SendToAll<T>(string path, Method method, T data, Status status = Status.Success);
        public Task SendToOne<T>(string UserId, string path, Method method, T data, Status status = Status.Success); 
    }
    public interface IManager<T>
    {
        public void Add(string Key, T Data);
        public bool TryGet(string Key, [MaybeNullWhen(false)] out T Data);
        public bool TryRemove(string Key, [MaybeNullWhen(false)] out T Data);
        public bool Contains(string Key);
    }

    public interface IPoolMannager : IManager<SalkyWebSocket>
    {
        public string ConnectionPoolId { get; }
        public void RemoveAllFromPool(string PoolId);
        public void RemoveFromConnectionPool(string PoolId, string UserId);
        public Task SendToAllInPool<T>(string PoolId, string Path, Method Method, T Data, Status Status = Status.Success) where T : notnull;
        public Task SendToOneInPool<T>(string PoolId, string UserId, string Path, Method Method, T Data, Status Status = Status.Success) where T : notnull;
        public void AddInPool(string PoolId, params string[] UsersId);

        public IEnumerable<KeyValuePair<string,Storage>> GetStorageOfManyInPool(string PoolId);
    }

    public class ConnectionPool
    {
        public string PoolId { get; }
    }

}