using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocket.Handler.Extensions;
using WebSocket.Handler.Interfaces;
using WebSocket.Shared;
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

   
    private async Task<MessageServer?> ReceiveSingleAsyncOrThrow(System.Net.WebSockets.WebSocket socket, TimeSpan MaxTime)
    {
        var timeCounter = new Stopwatch();
        timeCounter.Start();
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var buffer = new byte[1024 * 10];
        var receiveTask = socket.ReceiveAsync(buffer, token);
        while (!receiveTask.IsCompleted && timeCounter.Elapsed < MaxTime)
        {
            await Task.Delay(500);
        }
        if (!receiveTask.IsCompleted)
        {
            tokenSource.Cancel();
            await socket.CloseAsync(WebSocketCloseStatus.ProtocolError, "TIME OUT", CancellationToken.None);
            throw new TimeoutException("TIME OUT");
        }
        else
        {
            var result = await receiveTask;
            var fullMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            return JsonSerializer.Deserialize<MessageServer>(fullMessage);
        }
    }
    //Tem que se verficar a autoridade da chave publica,
    //enviando um dado aleatorio e esperando a resposta pra fazer o Match.
    //Caso contrario fecha a conexão e uma exceção.
    private async Task<SalkyWebSocketServer> PublicKeyValidationStep(SalkyWebSocketServer salkyWebSocketServer)
    {
        var MaxTime = TimeSpan.FromSeconds(2);
        //CRIA O DADO ALEATORIO
        var DataToEncrypt = $"{Guid.NewGuid()}{Guid.NewGuid()}{Guid.NewGuid()}";
        //ENCRIPTA O DADO
        var DataEncrypted = RsaService.FromPublicKey(salkyWebSocketServer.user.PublicKey).Encrypt(DataToEncrypt);
        //CRIA A MSG COM O DADO
        var msg = new MessageServer(salkyWebSocketServer.user.PublicKey, Encoding.UTF8.GetBytes("server"), "proveowner", DataEncrypted);
        //ENVA A MSG
        await salkyWebSocketServer.SendAsync(msg);
        //Espera pela resposta
        var response = await ReceiveSingleAsyncOrThrow(salkyWebSocketServer,MaxTime);
        //SE NULL FECHA
        if (response == null)
        {
            await salkyWebSocketServer.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid data", CancellationToken.None);
            throw new Exception("Invalid data");
        }
        else
        {
            //SE MANDOU NA ROTA ERRADA FECHA
            if(response.PathArray[0] != "proveowner")
            {
                await salkyWebSocketServer.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Invalid endpoint", CancellationToken.None);
                throw new Exception("Invalid endpoint");
            }
            //RETORNA O DADO PRA UTF8
            var ReturnedValue = Encoding.UTF8.GetString(response.Data);
            //VERIFICA SE É VALIDO, SE NÃO FECHA.
            if (ReturnedValue.Equals(DataToEncrypt))
            {
                return salkyWebSocketServer;
            }
            else
            {
                await salkyWebSocketServer.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid data", CancellationToken.None);
                throw new Exception("Invalid data");
            }
        }
    }
    //Tem que espera o client mandar o objeto UserSever.
    //Caso contrario fecha a conexão e lançar uma exceção;
    private async Task<SalkyWebSocketServer> UserServerStep(System.Net.WebSockets.WebSocket socket)
    {
        SalkyWebSocketServer? retorno;
        var MaxTime = TimeSpan.FromSeconds(3);
        var msg = await ReceiveSingleAsyncOrThrow(socket, MaxTime);
        if(msg == null)
        {
            await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Invalid data", CancellationToken.None);
            throw new Exception("Invalid data");
        }
        if(msg.PathArray[0] != "setuser")
        {
            await socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "Invalid endpoint", CancellationToken.None);
            throw new Exception("Invalid endpoint");
        }
        var userServer = JsonSerializer.Deserialize<UserServer>(msg.Data);
        if(userServer == null || !userServer.IsValid())
        {
            await socket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "The UserServer cannot be null or invalid", CancellationToken.None);
            throw new Exception("The UserServer cannot be null or invalid");
        }
        //Garatir um nome unico, porém não vai garatir uma unica chave publica
        //Fazendo com que o usuario que
        //logou na conta em um app e logou na mesma em outra app, fique com nicks distintos
        //Rever este comportamento . .
        if (_connectionManager.Any(userServer.Apelido))
        {
            var rng = new Random();
            var newName = $"{userServer.Apelido}-{rng.Next(int.MaxValue)}";
            userServer.Apelido = newName;
        }
        retorno = new SalkyWebSocketServer(socket, userServer);
        var userJson = JsonSerializer.SerializeToUtf8Bytes(retorno.user);
        await retorno.SendAsync(new MessageServer(retorno.user.PublicKey, Encoding.UTF8.GetBytes("server"), "setuser", userJson));
        return retorno; 
    }
    private async Task<SalkyWebSocketServer> TryDoHandShake(System.Net.WebSockets.WebSocket socket)
    {
        var salkySocket = await UserServerStep(socket);
        salkySocket = await PublicKeyValidationStep(salkySocket);
        return salkySocket;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            Console.WriteLine("!!! WEBSOCKET CONNECTION RECEIVED !!!");
            var pureWebSocket = await context.WebSockets.AcceptWebSocketAsync();
            SalkyWebSocketServer? salkySocket = null;
            try
            {
                salkySocket = await TryDoHandShake(pureWebSocket);
                salkySocket = _connectionManager.Add(salkySocket);
                await ReceiveMessage(salkySocket);
            }
            catch(Exception ex)
            {
                Console.WriteLine("=!=!=!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=ERROR-INI!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=");
                Console.WriteLine(ex.Message);                             
                Console.WriteLine(ex.ToString());                          
                Console.WriteLine("=!=!=!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=ERROR-END!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=!==!=!=");
                if (salkySocket is not null)
                    _ = _connectionManager.TryRemoveBySocketGUIDID(salkySocket.GUIDID, out var removedSocket);
                if (pureWebSocket.State == WebSocketState.Open)
                    await pureWebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Error", CancellationToken.None);
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
            Console.WriteLine("! Message reiceved");
            switch (result.MessageType)
            {
                case WebSocketMessageType.Text:
                    await MessageTextHandler(result, buffer, webSocket);
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
        Console.WriteLine("! Message text received, text : ");
        Console.WriteLine(fullMessage);
        var msg = JsonSerializer.Deserialize<MessageServer>(fullMessage);
        switch (msg?.PathArray[0])
        {
            case "route":
                await route(websocket, msg);
                break;
            case "getuser":
                await getuser(websocket,msg);
                break;
            case "getusers":
                await getusers(websocket);
                break;
            default:
                await error(websocket, $"Cannot complete the request, Invalid value for {nameof(MessageServer.PathString)}");
                break;
        }
    }
    private async Task getuser(SalkyWebSocketServer websocket, MessageServer msg)
    {
        var name = Encoding.UTF8.GetString(msg.Data);
        var user = _connectionManager.FindByUniqueName(name)?.user;
        byte[]? userJson = JsonSerializer.SerializeToUtf8Bytes(user);
        var message = new MessageServer(websocket.user.PublicKey, Encoding.UTF8.GetBytes("server"), "getuser", userJson);
        await websocket.SendAsync(message);
    }
    public async Task route(SalkyWebSocketServer ws, MessageServer message)
    {
        var target = _connectionManager.FindByPublicKey(message.ReceiverPublicKey);
        if (target == null)
            await error(ws, "Usuario não encontrado");
        else
            await target.SendAsync(message);
    }
    internal async Task getusers(SalkyWebSocketServer ws) 
    {
        var users = _connectionManager.GetAllVisible();
        var usersJson = JsonSerializer.SerializeToUtf8Bytes(users);
        var message = new MessageServer(ws.user.PublicKey,Encoding.UTF8.GetBytes("server"),"getusers",usersJson);
        await ws.SendAsync(message);
    }
    
    internal async Task MessageCloseHandler(SalkyWebSocketServer webSocket)
    {
        Console.WriteLine("Receive close msg");
        _connectionManager.TryRemove(webSocket.user.PublicKey, out SalkyWebSocketServer? removedSocket);
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
    }
    private async Task error(SalkyWebSocketServer ws,string errorMesage)
    {
        var message = new MessageServer(ws.user.PublicKey, Encoding.UTF8.GetBytes("server"), "error", JsonSerializer.SerializeToUtf8Bytes(errorMesage));
        await ws.SendAsync(message);
    }
    
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
