
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Salky.App.Services;
using Salky.Domain;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;

namespace Salky.WebSocket.Infra.Routing
{
    public class WebSocketRouteBase
    {
        private IConnectionManager connectionMannager;

        public WebSocketRouteBase()
        {

        }

        public async Task SendBack<T>(SalkyWebSocket ws ,T data,string path, Method method)
        {
            var msgServer = new MessageServer()
            {
                DataJson = System.Text.Json.JsonSerializer.Serialize(data),
                MethodEnum = method,
                PathString = path,
                SenderIntentifier = "server",
            };
            await ws.SendMessageServer(msgServer);
        }
        public async Task SendErrorBack(SalkyWebSocket ws ,Exception ex)
        {
            await SendBack(
                ws:ws, 
                data:ex.Message, 
                path:"error", 
                method:Method.POST);
        }

        public async Task<MessageStatus> SendTo<T>(SalkyWebSocket ws, Guid targetUserId, T data, string path, Method method)
        {
            var conMann = GetConnectionMannager(ws);
            var usrConnected = conMann.TryGetByKey(targetUserId);
            if (usrConnected == null) return MessageStatus.notsended;
            await usrConnected.SendMessageServer(new MessageServer
            {
                DataJson = JsonConvert.SerializeObject(data),
                MethodEnum = method,
                PathString = path,
                SenderIntentifier = ws.user.Id.ToString(),
            });
            return MessageStatus.sended;
        }

        private IConnectionManager GetConnectionMannager(SalkyWebSocket ws)
        {
            if (this.connectionMannager == null)
            {
                var serviceProvider = ws.Storage.Get<IServiceProvider>();
                this.connectionMannager = serviceProvider.GetRequiredService<IConnectionManager>();
            }
            return this.connectionMannager;
        }

    }
}
