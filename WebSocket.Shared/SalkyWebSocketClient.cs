using Microsoft.Win32;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WebSocket.Shared.DataAcess.Models;
using WebSocket.Shared.Models;



namespace WebSocket.Shared
{
        
    public class SalkyWebSocketClient :  IDisposable
    {
        private ClientWebSocket clientWebSocket;
        private Func<UserServer> userGetter;
        private Action<UserServer> userSetter;
        private RsaService rsaService;
        public bool IsConnected => clientWebSocket != null && clientWebSocket.State == WebSocketState.Open;
        public bool IsConnecting => clientWebSocket != null && clientWebSocket.State == WebSocketState.Connecting;
        public bool IsClosed => clientWebSocket != null && clientWebSocket.State == WebSocketState.Closed;
        public WebSocketState ConnectionState => clientWebSocket.State;
        private Uri connectionUri;
        private event EventHandler<MessageServer> OnMessageReceived;
        private event EventHandler<string> OnClose;
        //Esses eventos podem ser pouco perfomaticos já que esperam a execução da função
        private ConcurrentDictionary<string, List<Action<MessageServer>>> events = new();
        private ConcurrentDictionary<string, List<Action<MessageServer>>> temporary_events = new();
        public UserServer CurrentUser
        {
            get => userGetter();
            set => userSetter(value);
        }



        private SalkyWebSocketClient(Func<UserServer> getuser, Action<UserServer> setuser,byte[] privateKey, Uri uri)
        {
            userGetter = getuser;
            userSetter = setuser;
            connectionUri = uri;
            clientWebSocket = new ClientWebSocket();
            OnMessageReceived += ExecuteTemporaryEvents;
            OnMessageReceived += ExecuteEvents;

            this.rsaService = RsaService.FromPrivateKey(privateKey);
            privateKey = null;
        }


        public static async Task<SalkyWebSocketClient> StartNewAsync(Func<UserServer> GetUser, Action<UserServer> SetUser,byte[] privateKey, Uri uri)
        {
            var salkySoc = new SalkyWebSocketClient(GetUser, SetUser, privateKey, uri);
            await salkySoc.clientWebSocket.ConnectAsync(salkySoc.connectionUri, CancellationToken.None);
            salkySoc.StarReceiveMessage();
            await salkySoc.DoHandShake();
            return salkySoc;
        }
        private async void StarReceiveMessage()
        {
            while (clientWebSocket.State == WebSocketState.Open)
            {
                try
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
                            OnClose?.Invoke(this, clientWebSocket.CloseStatusDescription ?? $"{clientWebSocket.CloseStatus}");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error");
                    Console.WriteLine(ex.Message);   
                    Console.WriteLine(ex.ToString());
                }
            }
        }

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
       
        public void OnRoute(string path, Action<MessageServer> p)
        {
            if (events.ContainsKey(path))
                events[path].Add(p);
            else
                if (!events.TryAdd(path, new List<Action<MessageServer>> { p }))
                throw new Exception("Não foi possivels adicionar o evento");
        }

        public async Task<MessageServer?> SendThenWaitResponse(MessageServer msg,string PathToWait, TimeSpan timeout)
        {
            var taskWait = WaitForAsync(PathToWait, timeout);
            await this.SendMessageServer(msg);
            return await taskWait;
        }
        public async Task<T?> SendThenWaitResponse<T>(MessageServer msg, string PathToWait, TimeSpan timeout) where T : class
        {
            var response = await SendThenWaitResponse(msg,PathToWait,timeout);
            if(response == null)
                return null;
            var parsed = JsonSerializer.Deserialize<T>(response.Data);
            return parsed;
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
        

       

        public void OnPost<T>(Action<T,MessageServer> handler)
        {
            var route = $"route/post/{typeof(T).Name}";
            OnRoute(route, x =>
            {
                var content = JsonSerializer.Deserialize<T>(x.Data);
                handler(content,x);
            });
        }
        public void OnGet<T>(Func<T> getter)
        {
            var route = $"route/get/{typeof(T).Name}";
            var routeToDo = $"route/post/{typeof(T).Name}";
            OnRoute(route, async x =>
            {
                var data = getter();
                var dataArr = JsonSerializer.SerializeToUtf8Bytes(data);
                var message = new MessageServer(x.SenderPublicKey, CurrentUser.PublicKey, routeToDo, dataArr);
                await SendMessageServer(message);
            });
        }

        public async Task SendPost<T>(T data, byte[] reiceverPublicKey)
        {
            var route = $"route/post/{typeof(T).Name}";
            var dataArr = JsonSerializer.SerializeToUtf8Bytes(data);
            var message = new MessageServer(reiceverPublicKey,CurrentUser.PublicKey,route,dataArr);
            await SendMessageServer(message);
        }
        
        public async Task SendGet<T>(byte[] reiceverPublicKey)
        {
            var route = $"route/get/{typeof(T).Name}";
            var msg = new MessageServer(reiceverPublicKey, this.CurrentUser.PublicKey ,route, new byte[0]);
            await SendMessageServer(msg);
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
            var messageServer = new MessageServer(Encoding.UTF8.GetBytes("server"),CurrentUser.PublicKey,"getuser", Encoding.UTF8.GetBytes(searchUserText));
            var responseTask = WaitForAsync("getuser", TimeSpan.FromSeconds(4));
            await SendMessageServer(messageServer);
            var response = await responseTask;
            if (response?.Data == null)
                return null;
            else
                return JsonSerializer.Deserialize<UserServer>(response.Data);
        }
        private async Task DoHandShake()
        {
            var proveownerListerner = WaitForAsync("proveowner", TimeSpan.FromSeconds(30));
            await SendThenSetUser();
            await ProvePublicKey(proveownerListerner);
        }
        private async Task ProvePublicKey(Task<MessageServer?> proveownerListerner)
        {
            var response = await proveownerListerner;
            if (response != null)
            {
                var dataToDecrypt = response.Data;
                var dataDecrypted = rsaService.Decrypt(dataToDecrypt);
                var message = new MessageServer(Encoding.UTF8.GetBytes("server"), CurrentUser.PublicKey, "proveowner", Encoding.UTF8.GetBytes(dataDecrypted));
                await SendMessageServer(message);
            }
        }
        private async Task SendThenSetUser()
        {
            var userJson = JsonSerializer.SerializeToUtf8Bytes(CurrentUser);
            var messageServer = new MessageServer(Encoding.UTF8.GetBytes("server"), CurrentUser.PublicKey, "setuser", userJson);
            var responseTask = WaitForAsync("setuser", TimeSpan.FromSeconds(10));
            await SendMessageServer(messageServer);
            var response = await responseTask;
            CurrentUser = JsonSerializer.Deserialize<UserServer?>(response.Data) ?? throw new($"Cannot deserialize into {nameof(UserServer)} at {nameof(SendThenSetUser)}");
        }

        public void Dispose()
        {
            clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client close", CancellationToken.None).RunSynchronously();
            clientWebSocket.Dispose();
            clientWebSocket = null;
            connectionUri = null;
            userGetter = null;
            userSetter = null;
            rsaService.Dispose();
            rsaService = null;
            events.Clear();
            temporary_events.Clear();
            events = null;
            temporary_events = null;
            OnMessageReceived = null;
            GC.Collect();
        }


    }
}
