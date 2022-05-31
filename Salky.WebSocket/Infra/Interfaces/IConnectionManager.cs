using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
namespace Salky.WebSocket.Infra.Interfaces
{
    public interface IConnectionManager
    {
        /// <summary>
        /// The <see langword="Key"/> of pool, can be used to acess this pool on <see cref="Previus"/>
        /// </summary>
        public string ConnectionPoolId { get; }
        /// <summary>
        /// The amount of <see cref="SalkyWebSocket"/> connected
        /// </summary>
        public int ConnectionsCount { get; }
        /// <summary>
        /// The amount of <see cref="IConnectionManager"/> as pool
        /// </summary>
        public int PoolsCount { get; }

        public IConnectionManager Previus { get; }
        public void AddSocket(string key, SalkyWebSocket data);
        public bool TryGetSocket(string key,[NotNullWhen(true)] out SalkyWebSocket? sock);

        /// <summary>
        /// Esté metodo leva em consideração o <see cref="SalkyWebSocket.UniqueId"/>, que difere da chave utlizada na inserção do <see cref="IConnectionManager.AddSocket(string, SalkyWebSocket)"/>
        /// <para>Caso a <paramref name="key"/> esteja sendo utlizada pela mesma instancia <see cref="SalkyWebSocket"/>, será removido.</para>
        /// <para>Caso a <paramref name="key"/> já esteja sendo utlizada por outra instancia de <see cref="SalkyWebSocket"/> e o estado da conexão for <see cref="WebSocketState.Closed"/> ou <see cref="WebSocketState.CloseReceived"/> ou <see cref="WebSocketState.CloseSent"/>  , será removido.</para>
        /// <para>Caso a <paramref name="key"/> já esteja sendo utlizada por outra instancia de <see cref="SalkyWebSocket"/> e o estado da conexão for <see cref="WebSocketState.Open"/> , não irá remover.</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="SalkySocket"></param>
        /// <returns>True se removido, se não false</returns>
        public bool RemoveSocket(string key, SalkyWebSocket SalkySocket);
        public bool ContainsSocketKey(string key);
        public IConnectionManager GetOrCreateConnectionPool(string PoolId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="removedData"></param>
        /// <returns>True if removed</returns>
        public bool TryRemoveSocket(string key, [NotNullWhen(true)] out SalkyWebSocket? removedData);
        //public bool TryRemoveBy(Func<SalkyWebSocket, bool> selector, out SalkyWebSocket? removedData);
        public void ForEachSocket(Action<SalkyWebSocket> action);
        public IEnumerable<T> SelectManySocket<T>(Func<SalkyWebSocket,T> selector);
        public IEnumerable<SalkyWebSocket> WhereSocket(Func<SalkyWebSocket, bool> evaluate);
        public Task ForEachSocket(Func<SalkyWebSocket, Task> action);

        /// <summary>
        /// <list type="bullet">
        /// <item>Cria a connection pool</item>
        /// <item>Recupera e adiciona todos <see cref="SalkyWebSocket>"/> utilizando como referencia as chaves <paramref name="participantsId"/> em <see cref="IConnectionManager.AddSocket(string, SalkyWebSocket)"/></item>
        /// </list>
        /// </summary>
        /// <param name="connectionPoolId"></param>
        /// <param name="participantsId"></param>
        /// <returns>Uma nova pool <see cref="IConnectionManager"/>, tendo como chave <paramref name="connectionPoolId"/> </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IConnectionManager CreateConnectionPool(string connectionPoolId, params string[] participantsId);

        /// <summary>
        /// </summary>
        /// <param name="connectionPoolId"></param>
        /// <returns><see cref="ConnectionPool"/> relativa ao id</returns>
        ///<exception cref="KeyNotFoundException"></exception>
        public IConnectionManager GetConnectionPool(string connectionPoolId);

        public bool PoolExist(string poolId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionPoolId"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns></returns>
        public IConnectionManager RemoveConnectionPool(string connectionPoolId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionPoolId"></param>
        /// <param name="removedPool"></param>
        /// <returns></returns>
        public bool TryRemoveConnectionPool(string connectionPoolId, [NotNullWhen(true)] out IConnectionManager? removedPool);
        public bool TryGetConnectionPool(string connectionPoolId, [NotNullWhen(true)]out IConnectionManager? connectionPool);
        public IConnectionManager? NavigateTo(Func<IConnectionManager, bool> ValidatePool, params string[] PoolsKey);
        public Task<IConnectionManager?> NavigateTo(Func<IConnectionManager, Task<bool>> ValidatePool, params string[] PoolsKey);
        public IConnectionManager? NavigateTo(params string[] PoolsKey);
        public Task SendToAll<T>(string path, Method method, T data) where T : notnull;
        public Task SendToAll<T>(Func<SalkyWebSocket,bool> CanSendToThis, string path, Method method, T data) where T : notnull;
        public Task SendToAll<T>(Func<SalkyWebSocket,Task<bool>> CanSendToThis, string path, Method method, T data) where T : notnull;

       
    }
}