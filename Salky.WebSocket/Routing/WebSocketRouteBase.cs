
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Salky.WebSocket.Infra.Routing
{
    public class WebSocketRouteBase
    {
        public IPoolMannager RootConnectionMannager;
        public WebSocketRouteBase() 
        {
     
        }
        public SalkyWebSocket UserSocket { get; set; }
        public ConcurrentStorage Storage => UserSocket.Storage;

        public string CurrentPath = "";
        private IServiceProvider provider;

        public List<Claim> Claims => UserSocket.UserClaims;

        bool builded = false;
        internal void Constructor(SalkyWebSocket webSocket,IServiceProvider provider)
        {
            if (!builded)
            {
                this.provider = provider;
                this.RootConnectionMannager = provider.GetRequiredService<IPoolMannager>();
                this.UserSocket = webSocket;
                builded = true;
            }
        }
        internal void Inject(string CurrentPath)
        {
            this.CurrentPath = CurrentPath;
        }

        /// <summary>
        /// Send the <paramref name="data"/> to current client on <paramref name="path"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="path"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public async Task SendBack<T>(T data,string path, Method method,Status status = Status.Success) where T : notnull
        {
            await UserSocket.SendMessageServer(new MessageServer(path, method, status,data));
        }
        /// <summary>
        /// Send the error to current client on <paramref name="currentRoute"/> with <see cref="Method.ERROR"/> 
        /// </summary>
        /// <param name="currentRoute"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendErrorBack(string currentRoute,string message, Method method = Method.POST)
        {
            await SendBack(
                data:new
                {
                    message = message
                }, 
                path:currentRoute, 
                method: method,
                status: Status.Error
                );
        }

        public void DeletePool(string PoolId)
        {
            this.RootConnectionMannager.RemoveAllFromPool(PoolId);
        }

        /// <summary>
        /// MUITOS PROBLEMAS DE ID EM QUEM CHAMA ESTE METODO
        /// </summary>
        /// <param name="PoolId"></param>
        /// <param name="SocketKey"></param>
        public void AddInPool(string PoolId,params string[] SocketKey)
        {
            RootConnectionMannager.AddInPool(PoolId, SocketKey);
        }

        public void TryRemoveFromPool(string PoolId,string SocketKey)
        {
            RootConnectionMannager.RemoveFromConnectionPool(PoolId,SocketKey);
        }

        public async Task SendToAllInPool<T>(string PoolId,string Path,Method method,T data) where T : notnull
        {
            await RootConnectionMannager.SendToAllInPool(PoolId, Path, method, data);
        }

        public async Task SendToOneInPool<T>(string PoolId,string ReceiverIndentifier, string Path, Method method, T data) where T : notnull
        {
            await RootConnectionMannager.SendToOneInPool(PoolId, ReceiverIndentifier,Path, method, data);
        }

    }
}
