
using Microsoft.Extensions.DependencyInjection;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using Salky.WebSocket.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Salky.WebSocket.Infra.Routing
{
    public class WebSocketRouteBase
    {
        public WebSocketRouteBase() { }

        public IPoolMannager RootConnectionMannager;
        public virtual Task OnConnectAsync() => Task.CompletedTask;
        public virtual Task OnDisconnectAsync() => Task.CompletedTask;
        public SalkyWebSocket UserSocket { get; set; }
        public ConcurrentStorage Storage => UserSocket.Storage;
        public Method CurrentRouteMethod { get; private set; }
        public string CurrentRoutePathClass { get; private set; } 
        public string CurrentPath = "";
        public List<Claim> Claims => UserSocket.UserClaims;

        bool builded = false;
        internal void Constructor(SalkyWebSocket webSocket,IServiceProvider provider)
        {
            if (!builded)
            {
                this.RootConnectionMannager = provider.GetRequiredService<IPoolMannager>();
                this.UserSocket = webSocket;
                builded = true;
            }
        }

        internal void Inject(RoutePath CurrentPath)
        {
            this.CurrentRouteMethod = CurrentPath.Method;
            this.CurrentRoutePathClass = CurrentPath.PathClass;
            this.CurrentPath = CurrentPath.Path;
        }

  
        public async Task SendBack<T>(T data,string path, Method method,Status status = Status.Success) where T : notnull
        {
            await UserSocket.SendMessageServer(new MessageServer(path, method, status,data));
        }
 
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

        public bool DeletePool(Key PoolId)
        {
            return this.RootConnectionMannager.RemoveAllFromPool(PoolId) != -1;
        }
        public int AddManyInPool(Key PoolId, params Key[] ClientsId)
        {
            return RootConnectionMannager.AddManyInPool(PoolId, ClientsId);
        }
        public bool AddOneInPool(Key PoolId, Key ClientKey)
        {
            return RootConnectionMannager.AddOneInPool(PoolId, ClientKey);
        }
        public bool RemoveOneFromPool(Key PoolKey, Key ClientKey)
        {
            return RootConnectionMannager.RemoveOneFromPool(PoolKey, ClientKey) > 0;
        }
        public async Task<int> SendToAllInPool<T>(Key PoolKey,string Path,Method method,T data) where T : notnull
        {
            return await RootConnectionMannager.SendToAllInPool(PoolKey, new MessageServer(Path,method,Status.Success,data));
        }
        public async Task<bool> SendToOneInPool<T>(Key PoolKey, Key ClientKey, string Path, Method method, T data) where T : notnull
        {
            return await RootConnectionMannager.SendToOneInPool(PoolKey, ClientKey, new MessageServer(Path, method, Status.Success, data));
        }

    }
}
