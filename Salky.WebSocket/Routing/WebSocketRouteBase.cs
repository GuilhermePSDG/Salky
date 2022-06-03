
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
        public IConnectionManager RootConnectionMannager;
        public WebSocketRouteBase()  { }
        public SalkyWebSocket UserSocket { get; set; }
        public ConcurrentStorage Storage => UserSocket.Storage;

        public string CurrentPath = "";

        public List<Claim> Claims => UserSocket.UserClaims;
        
        private void Inject(SalkyWebSocket webSocket,string CurrentPath)
        {
            this.UserSocket = webSocket;
            var serviceProvider = UserSocket.Storage.Get<IServiceProvider>();
            this.RootConnectionMannager = serviceProvider.GetRequiredService<IConnectionManager>();
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
        public void AddInPool(string PoolId,string SocketKey,SalkyWebSocket socket)
        {
            var pool = this.RootConnectionMannager.GetOrCreateConnectionPool(PoolId);
            if (pool.ContainsSocketKey(SocketKey))
            {
                if(!pool.RemoveSocket(SocketKey, socket))
                {
                    throw new InvalidOperationException("Duplicated Connection");
                }
            }
            pool.AddSocket(SocketKey, socket);
        }

        public bool TryRemoveFromPool(string PoolId,string SocketKey)
        {
            if(this.RootConnectionMannager.TryGetConnectionPool(PoolId,out var pool))
            {
                return pool.TryRemoveSocket(SocketKey,out var socket);
            }
            return false;
        }


        public IConnectionManager GetPool(string PoolId)
        {
            return RootConnectionMannager.NavigateTo(PoolId) ?? throw new NullReferenceException();
        }


    }
}
