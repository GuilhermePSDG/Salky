using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Salky.App.Services;
using Salky.WebSocket.Extensions;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Salky.WebSocket.Handler;

public partial class WebServerSocketMiddleware
{
    internal  RequestDelegate _next;
    internal  IConnectionManager _connectionManager;
    private ISalkyRouter router;

    public WebServerSocketMiddleware(RequestDelegate next, IConnectionManager connectionManager)
    {
        _next = next;
        this._connectionManager = connectionManager;
    }
    public async Task InvokeAsync(HttpContext context, ISecurityProvider securityProvider , ISalkyRouter salkyRouter, UserService userService, IServiceScope scopedService)
    {
        this.router = salkyRouter;
        if (context.WebSockets.IsWebSocketRequest)
        {
            SalkyWebSocket? salkySocket = null;
            try
            {
                var claims = securityProvider.ValidateJwtToken(context);
                var userId = claims.FirstOrDefault(f => f.Type == "nameid")?.Value;
                if (userId == null)
                {
                    await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Invalid token"));
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    throw new Exception("Invalid token");
                }
                var usr = await userService.GetUserById(Guid.Parse(userId));
                if (usr == null) throw new Exception("Usuario não encontrado");
                //
                salkySocket = new SalkyWebSocket(await context.WebSockets.AcceptWebSocketAsync(), usr);
                salkySocket.Storage.Add(scopedService.ServiceProvider);
                salkySocket = _connectionManager.Add(salkySocket.user.Id, salkySocket);
                await ReceiveMessage(salkySocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                if (salkySocket is not null)
                {
                    if (!_connectionManager.TryRemoveBy(f => f.UniqueId == salkySocket.UniqueId, out var removedSocket))
                        Console.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n" +
                                          "CANNOT REMOVE THE SOCKET FROM CONNECTION MANAGER\n" +
                                          "CANNOT REMOVE THE SOCKET FROM CONNECTION MANAGER\n" +
                                          "CANNOT REMOVE THE SOCKET FROM CONNECTION MANAGER\n" +
                                          "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    salkySocket.Abort();
                }
            }
        }
        else
        {
            await _next(context);
        }
    }
    internal async Task ReceiveMessage(SalkyWebSocket webSocket)
    {
        var buffer = new byte[1024 * 10];
        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult? result = await webSocket.ReceiveAsync(buffer,CancellationToken.None);
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
    internal async Task MessageTextHandler(WebSocketReceiveResult result, byte[] buffer, SalkyWebSocket websocket)
    {
        MessageServer msg = null;
        try 
        { 
            var fullMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            var msgResult = JsonSerializer.Deserialize<MessageServer>(fullMessage, jsonOptions);
            if (msgResult == null) throw new Exception("Message cannot be null");
            msg = msgResult;
        }
        catch(Exception ex)
        {
            await websocket.SendErrorAsync("You must send a object like this, and here a json of any type",ex);
        }
      
        try
        {
            if (msg != null)
                router.Route(websocket, msg);
        }
        catch(Exception ex)
        {
            await websocket.SendErrorAsync("Cannot complete your request", ex);
        }
    }

    internal async Task MessageCloseHandler(SalkyWebSocket webSocket)
    {
        _connectionManager.TryRemove(webSocket.user.Id, out SalkyWebSocket? removedSocket);
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Requested by client");
    }

}
