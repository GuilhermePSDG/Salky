using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocket;
using WebSocket.Handler.Extensions;
using WebSocket.Handler.Interfaces;
using WebSocket.Shared.Models;

namespace WebSocket.Handler;

public partial class WebServerSocketMiddleware
{
    internal readonly RequestDelegate _next;
    internal readonly IConnectionManager _connectionManager;
    public WebServerSocketMiddleware(RequestDelegate next, IConnectionManager connectionManager)
    {
        _next = next;
        this._connectionManager = connectionManager;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            var ws = _connectionManager.Add(new SalkyWebSocketServer(webSocket));
            try
            {
                await ReceiveMessage(ws);
            }
            catch
            {
                var removed = _connectionManager.TryRemove(ws.Id, out var removedSocket);
                if (ws.State == WebSocketState.Open)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.InternalServerError, "Error", CancellationToken.None);
                }
                throw;
            }
        }
        else
        {
            await _next(context);
        }
    }
    internal async Task ReceiveMessage(SalkyWebSocketServer webSocket)
    {
        var buffer = new byte[1024 * 10];
        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult? result = await webSocket.ReceiveAsync(buffer);
            Console.WriteLine("Message reiceved");
            switch (result.MessageType)
            {
                case WebSocketMessageType.Text:
                    await MessageTextHandler(result, buffer, webSocket);
                    _connectionManager[webSocket.Id] = webSocket;
                    break;
                case WebSocketMessageType.Close:
                    await MessageCloseHandler(webSocket);
                    break;
            }
        }
    }
    internal async Task MessageTextHandler(WebSocketReceiveResult result, byte[] buffer, SalkyWebSocketServer websocket)
    {
        var fullMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
        Console.WriteLine("Message text received, text : ");
        Console.WriteLine(fullMessage);
        var msg = JsonSerializer.Deserialize<MessageServer>(fullMessage);
        switch (msg?.PathArray[0])
        {
            case "route":
                await EnviarMensagemRoteada(websocket, msg);
                break;
            case "setuser":
                await ReceiveUserThenSendUser(websocket, msg);
                break;
            case "getuser":
                await SendUser(websocket,msg);
                break;
            case "getusers":
                await SendUsers(websocket);
                break;
            default:
                await SendError(websocket, $"Cannot complete the request, Invalid value for {nameof(MessageServer.PathArray)}");
                break;
        }
    }
    private async Task SendUser(SalkyWebSocketServer websocket, MessageServer msg)
    {
        var user = _connectionManager.FindByUniqueName(msg.Json)?.user;
        ///Pode causar problemas, porém não deve ser enviado . . .
        string? userJson = null;
        if(user != null)
        {
            user.ConnectionKey = "";
            var userjson = JsonSerializer.Serialize(user);
        }
        var message = new MessageServer(websocket.user?.Apelido ?? "Unknow user", "server", "getuser", userJson);
        await websocket.SendAsync(message);
    }
    public async Task EnviarMensagemRoteada(SalkyWebSocketServer ws, MessageServer message)
    {
        var target = _connectionManager.FindByUniqueName(message.Receiver);
        if (target == null)
            await SendError(ws, "The user cannot be founded");
        else
            await target.SendAsync(message);
    }
    internal async Task SendUsers(SalkyWebSocketServer ws) 
    {
        var users = _connectionManager.GetAllVisible();
        ///Pode causar problemas, porém não deve ser enviado . . .
        users.ForEach(q => q.ConnectionKey = "");
        var usersJson = JsonSerializer.Serialize(users);
        var message = new MessageServer(ws.user?.Apelido ?? "Unknow User", "server", "getusers", usersJson);
        await ws.SendAsync(message);
    }
    internal async Task ReceiveUserThenSendUser(SalkyWebSocketServer websocket, MessageServer msg)
    {
        UserServer? reicevedUser = JsonSerializer.Deserialize<UserServer>(msg.Json);
        if (reicevedUser == null)
        {
            await SendError(websocket, "The users cannot be null");
        }
        else
        {
            //----ISSO AQUI DEVERIA SER RESPONSABILIDADE DO CONNECTIONMANNAGER----//
            //Caso já exista alguém com esse nome e que não seja o propio
            //Modifica o nome para garantir que seja unico.
            if(!websocket.user?.Apelido.Equals(reicevedUser.Apelido) ?? true)
            {
                if (_connectionManager.Any(reicevedUser.Apelido))
                {
                    var rng = new Random();
                    var newName = $"{reicevedUser.Apelido}-{rng.Next(int.MaxValue)}";
                    reicevedUser.Apelido = newName;
                }
            }
            websocket.user = reicevedUser;
            websocket.user.ConnectionKey = websocket.Id;
            var userJson = JsonSerializer.Serialize(websocket.user);
            var message = new MessageServer(websocket.user.Apelido, "server", "setuser", userJson);
            await websocket.SendAsync(message);
            Console.WriteLine("Message user sended");
            _connectionManager[websocket.Id] = websocket;
        }
    }
    internal async Task MessageCloseHandler(SalkyWebSocketServer webSocket)
    {
        Console.WriteLine("Receive close msg");
        _connectionManager.TryRemove(webSocket.Id, out SalkyWebSocketServer? removedSocket);
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
    }
    private async Task SendError(SalkyWebSocketServer ws,string errorMesage)
    {
        var message = new MessageServer(ws.user?.Apelido ?? "Unknow user", "Unknow", "error", JsonSerializer.Serialize(errorMesage));
        await ws.SendAsync(message);
    }
    public void EnviarParaTodos(MessageServer msg) => _connectionManager.ForEach(async sock => await sock.SendAsync(msg));
    internal void EscreverParametrosDoRequest(HttpContext context)
    {
        Console.WriteLine($"Request method : {context.Request.Method}");
        Console.WriteLine($"Request protocol : {context.Request.Protocol}");
        if (context.Request.Headers is not null)
        {
            foreach (var h in context.Request.Headers)
            {
                Console.WriteLine($"--> {h.Key} : {h.Value}");
            }
        }
    }
}
