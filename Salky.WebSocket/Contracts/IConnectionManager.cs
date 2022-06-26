using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using Salky.WebSocket.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
namespace Salky.WebSocket.Infra.Interfaces
{

    public interface IWebSocketMannager
    {
        public void AddSocket(Key Key, SalkyWebSocket ws);
        public bool TryGetSocket(Key Key, [MaybeNullWhen(false)] out SalkyWebSocket ws);
        public bool TryRemoveSocket(Key Key, [MaybeNullWhen(false)] out SalkyWebSocket ws);
        public bool ContainsSocketKey(Key Key);
    }

    public interface IPoolMannager : IWebSocketMannager
    {
        public string ConnectionPoolId { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PoolKey"></param>
        /// <returns>-1 if not exist, otherwise the removed count</returns>
        public int RemoveAllFromPool(Key PoolKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PoolKey"></param>
        /// <param name="ClientToRemoveKey"></param>
        /// <returns>-1 if not exist, 0 if not removed/not found, 1 if removed </returns>
        public int RemoveOneFromPool(Key PoolKey, Key ClientKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PoolKey"></param>
        /// <param name="message"></param>
        /// <returns>-1 if not exist, or the sended count</returns>
        public Task<int> SendToAllInPool(Key PoolKey, MessageServer message);
        public Task<bool> SendToOneInPool(Key PoolKey, Key ClientKey, MessageServer message);
        public bool AddOneInPool(Key PoolKey, Key ClientKey);
        /// <summary>
        /// </summary>
        /// <param name="PoolKey"></param>
        /// <param name="ClientsKey"></param>
        /// <returns>The amount of clients added in pool</returns>
        public int AddManyInPool(Key PoolKey, Key[] ClientsKey);
        public IEnumerable<KeyValuePair<string,Storage>> GetStorageOfManyInPool(Key PoolKey);
    }

}