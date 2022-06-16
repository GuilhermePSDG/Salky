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
using System.Web;

namespace Salky.WebSocket.Handler;

public partial class WebServerSocketMiddleware
{
    private RequestDelegate _next;
    private readonly IConnectionManager _connectionManager;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILogger<WebServerSocketMiddleware> logger;

    public WebServerSocketMiddleware(RequestDelegate next, IConnectionManager connectionManager, IServiceScopeFactory scopeFactory, ILogger<WebServerSocketMiddleware> logger)
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
                    //Caso não esteja fechado, irá fechar
                    if(oldSocket.State == WebSocketState.CloseReceived || oldSocket.ConnectionsIsOpen)
                    {
                        await oldSocket.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, CloseDescription.DuplicatedConnection);
                    }
                    var removed = _connectionManager.RemoveSocket(key, oldSocket);
                    this.logger.LogInformation(removed ? "Conexão removida" : "Conexão NÃO FOI removida");
                }
                _connectionManager.AddSocket(key,salkySocket);
                addedToPool = true;
                salkySocket.Storage.Add(serviceProvider);
                await serviceProvider.GetRequiredService<IRouterResolver>().AfterOpen(salkySocket);
                (_connectionManager as ConnectionMannager).PrintDeep();
                await ReceiveMessage(salkySocket, key);
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
                        if (_connectionManager.TryRemoveSocket(key, out var removedsock))
                        {
                            if(removedsock.State == WebSocketState.Open)
                            {
                                await removedsock.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, CloseDescription.Unknow);
                                removedsock.Dispose();
                            }

                        }

                    }
                }
            }
            (_connectionManager as ConnectionMannager).PrintDeep();
        }
        else if (context.Request.Path.Equals("/ws/doc"))
        {
            var routes = RouteResolver.RouteDocs();
            var style = @"<style>
.card{
    background:#f3f3f3;
    padding:10px;
    margin:15px;
}
</style>";
            var template =
@"
<div class=""card"">
    <p>Path : $Path</p>
    <p>Method : $Method</p>
    <p>Parameter : $ParameterType</p>
    <p>Parameter Json : $ParameterJson</p>
</div>
";
            var html = new StringBuilder();
            html.Append(style);
            html.Append(@"<div style=""display:flex;flex-wrap:wrap;"">");
            foreach(var route in routes)
            {
                html.AppendLine(template
                    .Replace($"${nameof(RouteResolver.RouteDoc.Path)}",route.Path)
                    .Replace($"${nameof(RouteResolver.RouteDoc.Method)}",route.Method)
                    .Replace($"${nameof(RouteResolver.RouteDoc.ParameterType)}",route.ParameterType)
                    .Replace($"${nameof(RouteResolver.RouteDoc.ParameterJson)}", JsonSerializer.Serialize(route.ParameterJson))
                    );
            }
            html.Append(@"</div>");

            await context.Response.WriteAsync(html.ToString());

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
        await webSocket.Storage.Get<IServiceProvider>().GetRequiredService<IRouterResolver>().Closed(webSocket);
        if(!_connectionManager.RemoveSocket(key, webSocket))
        {
            this.logger.LogDebug("CONEXÃO NÃO REMOVIDA");
        }
        webSocket.Dispose();
    }

}
