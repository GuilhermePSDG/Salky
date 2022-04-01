using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WebSocket.Shared;
using WebSocket.Shared.DataAcess.Models;
using WebSocket.Shared.Models;
namespace Wpf.MVVM.ViewModels
{
    public class SalkyWebSocketClient
    {
        private System.Net.WebSockets.ClientWebSocket clientWebSocket;
        private event EventHandler<MessageServer> OnMessageReceived;
        private SalkyWebSocketClient()
        {
            clientWebSocket = new System.Net.WebSockets.ClientWebSocket();

        }
        public static async Task<SalkyWebSocketClient> StartNewAsync(Func<UserServer> GetUser, Action<UserServer> SetUser, Uri uri)
        {
            var salkySoc = new SalkyWebSocketClient();
            await salkySoc.clientWebSocket.ConnectAsync(uri,CancellationToken.None);
            throw new NotImplementedException();
        }
        private async void StarReceiveMessage()
        {
            while(clientWebSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                var buffer = new byte[1024*10];
                var result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                switch (result.MessageType)
                {
                    case System.Net.WebSockets.WebSocketMessageType.Text:

                        break;
                    case System.Net.WebSockets.WebSocketMessageType.Binary:
                        break;
                    case System.Net.WebSockets.WebSocketMessageType.Close:
                        break;
                }

            }
            



        }

        public void On(string v, Action<object> p)
        {
            throw new NotImplementedException();
        }

        public object WaitForAsync(string v, TimeSpan timeSpan)
        {
            throw new NotImplementedException();
        }

        public Task SendMessageServer(MessageServer messageServer)
        {
            throw new NotImplementedException();
        }

        public Task GetUser(string searchUserText)
        {
            throw new NotImplementedException();
        }

        public Task SendThenSetUser()
        {
            throw new NotImplementedException();
        }
    }
}
