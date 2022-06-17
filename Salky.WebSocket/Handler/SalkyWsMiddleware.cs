    using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Salky.WebSocket.Extensions;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace Salky.WebSocket.Handler;

public partial class SalkyWebSocketMiddleWare
{
    private RequestDelegate _next;
    private readonly IConnectionManager _connectionManager;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILogger<SalkyWebSocketMiddleWare> logger;

    public SalkyWebSocketMiddleWare(RequestDelegate next, IConnectionManager connectionManager, IServiceScopeFactory scopeFactory, ILogger<SalkyWebSocketMiddleWare> logger)
    {
        _next = next;
        this._connectionManager = connectionManager;
        this.scopeFactory = scopeFactory;
        this.logger = logger;
        logger.Log(LogLevel.Information, "WebSocket MiddleWare Started");
    }


    private void MakeHttpHandshake(HttpContext context, IServiceProvider serviceProvider, out List<Claim> Claims, out string SocketKey)
    {
        var httpHandShaker = serviceProvider.GetService(typeof(IDoHttpHandshake));
        if(httpHandShaker == null)throw new InvalidOperationException($"You must provide a class that inherits from {nameof(IDoHttpHandshake)}");
        ((IDoHttpHandshake)httpHandShaker).MakeOrThrow(context, out Claims, out SocketKey);
    }
    private void MakeWsHandShake(IServiceProvider serviceProvider,SalkyWebSocket ws)
    {
        var wsHandShaker = serviceProvider.GetService(typeof(IDoWebSocketHandShake));
        if (wsHandShaker != null) ((IDoWebSocketHandShake)wsHandShaker).MakeOrThrow(ws);
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            this.logger.Log(LogLevel.Information, "WebSocket Connection Received");
            var serviceProvider = this.scopeFactory.CreateScope().ServiceProvider;
            string? key = null;
            bool addedToPool = false;
            SalkyWebSocket? salkySocket = null;
            try
            {
                List<Claim> claims;
                    
                try
                {
                    MakeHttpHandshake(context,serviceProvider, out claims, out key);
                }
                catch(Exception ex)
                {
                    this.logger.LogInformation(ex, "Http HandShake Failed");
                    return;
                }
                salkySocket = new SalkyWebSocket(await context.WebSockets.AcceptWebSocketAsync(), claims);
                try
                {
                    MakeWsHandShake(serviceProvider, salkySocket);
                }
                catch (Exception ex)
                {
                    this.logger.LogInformation(ex, "WebSocket HandShake Failed");
                    await salkySocket.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation,CloseDescription.HandShakeProblems);
                    return;
                }


                if (_connectionManager.TryGetSocket(key,out var oldSocket))
                {
                    if(oldSocket.State == WebSocketState.CloseReceived || oldSocket.ConnectionsIsOpen)
                    {
                        await oldSocket.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, CloseDescription.DuplicatedConnection);
                    }
                    var removed = _connectionManager.RemoveSocket(key, oldSocket);
                    if (removed)
                        this.logger.LogDebug("Duplicated Connection Removed");
                    else
                        this.logger.LogError("Duplicated Connection NOT removed");
                }
                _connectionManager.AddSocket(key,salkySocket);
                addedToPool = true;
                salkySocket.Storage.Add(serviceProvider);
                await serviceProvider.GetRequiredService<IRouterResolver>().AfterOpen(salkySocket);
                (_connectionManager as ConnectionMannager).PrintDeep();
                await ReceiveMessage(salkySocket, key);
                await serviceProvider.GetRequiredService<IRouterResolver>().AfterClose(salkySocket);

            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while invoke middleware");
            }
            finally
            {
                if (addedToPool && key != null && salkySocket != null)
                {
                    if (_connectionManager.TryGetSocket(key, out var sock) && sock.UniqueId == salkySocket.UniqueId)
                    {
                        if (_connectionManager.TryRemoveSocket(key, out var removedsock) && removedsock.State == WebSocketState.Open)
                        {
                            this.logger.LogDebug("==========================================");
                            this.logger.LogDebug("UNEXPECTED SCENARIO - UNEXPECTED SCENARIO");
                            this.logger.LogDebug("==========================================");
                            try {removedsock.Abort();}catch { }
                            try {removedsock.Dispose(); }catch { }
                        }
                    }
                }
            }
            (_connectionManager as ConnectionMannager).PrintDeep();
        }
        else if (context.Request.Path.Equals("/ws.json"))
        {
            var routes = RouteResolver.RouteDocs();
            var json = JsonSerializer.Serialize(routes,
                new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Converters = { new JsonStringEnumConverter() }
                });
            await context.Response.WriteAsync(json);
        }
        else
        {
            await _next(context);
        }
    }
    internal async Task ReceiveMessage(SalkyWebSocket webSocket,string id)
    {
        try
        {
            var buffer = new byte[51200];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult? result = await webSocket.ReceiveAsync(buffer,CancellationToken.None);
                if (!result.EndOfMessage)
                {
                    await webSocket.SendErrorAsync("Too large message","error");
                    this.logger.LogWarning("Message Too large to receive");
                    buffer = new byte[51200];
                }
                else
                {
                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            await MessageTextHandler(result, buffer, webSocket);
                            break;
                        case WebSocketMessageType.Close:
                            await MessageCloseHandler(webSocket, result, id);
                            break;
                        case WebSocketMessageType.Binary:
                            await webSocket.SendErrorAsync("Binary Message Not Implemented","error");
                            this.logger.LogWarning("Binary Message Not Implemented");
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Erro ao receber mensagem");
        }
    }
 
    internal async Task MessageTextHandler(WebSocketReceiveResult result, byte[] buffer, SalkyWebSocket websocket)
    {
        try 
        {
            var fullMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var msgResult = JsonSerializer.Deserialize<MessageServer>(fullMessage, DefaultJsonSerializerOptions);
            if (msgResult == null) throw new Exception("Message cannot be null");
            await websocket.Storage.Get<IServiceProvider>().GetRequiredService<IRouterResolver>().Route(websocket, msgResult);
        }
        catch
        {
            this.logger.LogWarning("Error while receiving message");
            await websocket.SendErrorAsync("You must send a object like this, and here a object of any type","error");
        }
    }

    internal async Task MessageCloseHandler(SalkyWebSocket webSocket,WebSocketReceiveResult result, string key)
    {
        this.logger.LogInformation("Close Message Received");
        if (!Enum.TryParse<CloseDescription>(result.CloseStatusDescription, true, out var parsedDescription))
            parsedDescription = CloseDescription.Unknow;
        if(webSocket.State == WebSocketState.CloseReceived || webSocket.State == WebSocketState.Open)
            await webSocket.CloseOutputAsync(result.CloseStatus ?? WebSocketCloseStatus.Empty, parsedDescription);
        if(!_connectionManager.RemoveSocket(key, webSocket))
        {
            this.logger.LogDebug("CONEXÃO NÃO REMOVIDA");
        }
        webSocket.Dispose();
    }

}
