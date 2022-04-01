using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WebSocket.Shared.DataAcess.Models;
using WebSocket.Shared.Models;

namespace WebSocket.Shared
{
    public class SalkyWebSocketClient
    {
        private ClientWebSocket clientWebSocket;
        private Func<UserServer> userGetter;
        private Action<UserServer> userSetter;

        public bool IsConnected => clientWebSocket != null && clientWebSocket.State == WebSocketState.Open;
        public bool IsConnecting => clientWebSocket != null && clientWebSocket.State == WebSocketState.Connecting;
        public bool IsClosed => clientWebSocket != null && clientWebSocket.State == WebSocketState.Closed;
        public WebSocketState ConnectionState => clientWebSocket.State;
        
        
        public UserServer CurrentUser
        {
            get => userGetter();
            set => userSetter(value);
        }

        private Uri connectionUri;
        private event EventHandler<MessageServer> OnMessageReceived;
        private event EventHandler<string> OnClose;
        private SalkyWebSocketClient(Func<UserServer> getuser, Action<UserServer> setuser, Uri uri)
        {
            userGetter = getuser;
            userSetter = setuser;
            connectionUri = uri;
            clientWebSocket = new ClientWebSocket();
            OnMessageReceived += ExecuteTemporaryEvents;
            OnMessageReceived += ExecuteEvents;
        }
        public static async Task<SalkyWebSocketClient> StartNewAsync(Func<UserServer> GetUser, Action<UserServer> SetUser, Uri uri)
        {
            var salkySoc = new SalkyWebSocketClient(GetUser, SetUser, uri);
            await salkySoc.clientWebSocket.ConnectAsync(salkySoc.connectionUri, CancellationToken.None);
            salkySoc.StarReceiveMessage();
            await Task.Delay(1000);
            await salkySoc.SendThenSetUser();
            return salkySoc;
        }
        private async void StarReceiveMessage()
        {
            while (clientWebSocket.State == WebSocketState.Open)
            {
                var buffer = new byte[1024 * 10];
                var result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        var fullMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var messageServer = JsonSerializer.Deserialize<MessageServer>(fullMessage) ?? throw new Exception("Wrong type of message received");
                        OnMessageReceived.Invoke(this, messageServer);
                        break;
                    case WebSocketMessageType.Binary:
                        break;
                    case WebSocketMessageType.Close:
                        await clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Requested by the server", CancellationToken.None);
                        OnClose.Invoke(this, clientWebSocket.CloseStatusDescription ?? $"{clientWebSocket.CloseStatus}");
                        break;
                }
            }
        }

        //Esses eventos podem ser pouco perfomaticos já que esperam a execução da função
        private ConcurrentDictionary<string, List<Action<MessageServer>>> events = new();
        private ConcurrentDictionary<string, List<Action<MessageServer>>> temporary_events = new();
        private void ExecuteEvents(object? discart, MessageServer msg)
        {
            if (events.ContainsKey(msg.PathString))
                foreach (var func in events[msg.PathString])
                    func(msg);
        }
        private void ExecuteTemporaryEvents(object? discart, MessageServer msg)
        {
            temporary_events.TryRemove(msg.PathString, out var temp);
            if (temp != null)
                foreach (var func in temp)
                    func(msg);
        }
        public void On(string path, Action<MessageServer> p)
        {
            if (events.ContainsKey(path))
                events[path].Add(p);
            else
                if (!events.TryAdd(path, new List<Action<MessageServer>> { p }))
                throw new Exception("Não foi possivels adicionar o evento");
        }
        public async Task<MessageServer?> WaitForAsync(string path, TimeSpan timeSpan)
        {
            MessageServer? retorno = null;
            var timeCounter = new Stopwatch();
            timeCounter.Start();
            if (temporary_events.ContainsKey(path))
            {
                temporary_events[path].Add((x) => retorno = x);
            }
            else
            {
                if (!temporary_events.TryAdd(path, new List<Action<MessageServer>> { (x) => retorno = x }))
                    throw new Exception("Não foi possivels adicionar o evento");
            }
            while (retorno == null && timeCounter.Elapsed < timeSpan)
            {
                await Task.Delay(100);
            }
            return retorno;
        }
        public async Task SendMessageServer(MessageServer messageServer)
        {
            var json = JsonSerializer.Serialize(messageServer);
            var buffer = Encoding.UTF8.GetBytes(json);
            await clientWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None); ;
        }
        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            await clientWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }
        public async Task<UserServer?> GetUser(string searchUserText)
        {
            var messageServer = new MessageServer("server", CurrentUser.Apelido, "getuser", searchUserText);
            var responseTask = WaitForAsync("getuser", TimeSpan.FromSeconds(4));
            await SendMessageServer(messageServer);
            var response = await responseTask;
            if (response?.Json == null)
                return null;
            else
                return JsonSerializer.Deserialize<UserServer>(response.Json);
        }
        public async Task SendThenSetUser()
        {
            var userJson = JsonSerializer.Serialize(CurrentUser);
            var messageServer = new MessageServer("server", CurrentUser.Apelido, "setuser", userJson);
            var responseTask = WaitForAsync("setuser", TimeSpan.FromSeconds(4));
            await SendMessageServer(messageServer);
            var response = await responseTask;
            CurrentUser = JsonSerializer.Deserialize<UserServer?>(response.Json) ?? throw new($"Cannot deserialize into {nameof(UserServer)} at {nameof(SendThenSetUser)}");
        }

    }
}
