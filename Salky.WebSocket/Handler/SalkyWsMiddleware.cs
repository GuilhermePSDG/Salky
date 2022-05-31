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

    private void ValidateJwtToken(HttpContext context, IValidateToken tokenValidator,out List<Claim> Claims,out string socketKey)
    {
        var token = HttpUtility.ParseQueryString(context.Request.QueryString.Value).Get("token");
        if (string.IsNullOrEmpty(token)) throw new NullReferenceException("Jwt Token Not Found");
        Claims= tokenValidator.ValidateJwtTokenOrThrow(token, out socketKey);
    }
    private void MakeHttpHandshake(HttpContext context, IServiceProvider serviceProvider)
    {
        var httpHandShaker = serviceProvider.GetService(typeof(IDoHttpHandshake));
        if (httpHandShaker != null) ((IDoHttpHandshake)httpHandShaker).MakeOrThrow(context);
    }
    private void MakeWsHandShake(IServiceProvider serviceProvider,SalkyWebSocket ws)
    {
        var wsHandShaker = serviceProvider.GetService(typeof(IDoWebSocketHandShake));
        if (wsHandShaker != null) ((IDoWebSocketHandShake)wsHandShaker).MakeOrThrow(ws);
    }
    public async Task InvokeAsync(HttpContext context, IValidateToken tokenValidator)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {

            var value = context.Request.Cookies["teste"];
            if (value != null)
            {
                logger.LogInformation("Cookkie found, value : " + value);
            }
            else
            {
                logger.LogInformation("Cookkie not found" + value);
                context.Response.Cookies.Append("teste", "Hello");
            }

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
                    ValidateJwtToken(context, tokenValidator, out claims, out key);

                }
                catch(Exception e)
                {
                    this.logger.LogInformation(e,"WebSocket HandShake Failed");
                    return;
                }
                    
                try
                {
                    MakeHttpHandshake(context,serviceProvider);
                }
                catch(Exception ex)
                {
                    this.logger.LogInformation(ex, "WebSocket HandShake Failed");
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
                    await webSocket.SendErrorAsync("Too large message,not implemented");
                    this.logger.LogWarning("Message Too large received");
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
                            await webSocket.SendErrorAsync("Binary Message Not Implemented");
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
            await websocket.SendErrorAsync("You must send a object like this, and here a object of any type");
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
