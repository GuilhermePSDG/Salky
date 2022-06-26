using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Salky.WebSocket.Infra.Interfaces;
using Salky.WebSocket.Infra.Models;
using Salky.WebSocket.Infra.Socket;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Salky.WebSocket.Handler;

public partial class SalkyWebSocketMiddleWare
{
    private RequestDelegate _next;
    private readonly IPoolMannager _connectionManager;
    private readonly IServiceScopeFactory scopeFactory;
    private readonly ILogger<SalkyWebSocketMiddleWare> logger;

    public SalkyWebSocketMiddleWare(
        RequestDelegate next, 
        IPoolMannager connectionManager, 
        IServiceScopeFactory scopeFactory, 
        ILogger<SalkyWebSocketMiddleWare> logger
        )

    {
        _next = next;
        this._connectionManager = connectionManager;
        this.scopeFactory = scopeFactory;
        this.logger = logger;
        logger.Log(LogLevel.Information, "WebSocket MiddleWare Started");
    }


    private bool MakeHttpHandshake(HttpContext context, IServiceProvider serviceProvider, [NotNullWhen(true)]out List<Claim>? Claims , [NotNullWhen(true)] out string? SocketKey )
    {
        Claims = null;
        SocketKey = null;
        var identityGuard = serviceProvider.GetService<HttpWebSocketGuardIdentityProvider>();
        if (identityGuard != null && !identityGuard.CanContinue(context, out Claims, out SocketKey))
            return false;
        foreach (var guard in serviceProvider.GetServices<HttpWebSocketGuard>())
            if (!guard.CanContinue(context,Claims,SocketKey))
                return false;
        if (Claims == null) Claims = new();
        if (SocketKey == null) SocketKey = Guid.NewGuid().ToString();
        return true;
    }
   
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            await _next(context);
            return;
        }
        this.logger.LogInformation("WebSocket Connection Received");
        SalkyWebSocket? ws = null;
        using var scope = this.scopeFactory.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        try
        {
            ws = await TryAcceptConnection(context, serviceProvider);
            if (ws == null)
            {
                this.logger.LogInformation("Connection denied.");
                return;
            }
            await AfterConnectionEstablished(ws, serviceProvider);
            await ReceiveMessage(ws, serviceProvider);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error while invoke middleware");
        }
        finally
        {
            if (ws != null)
                await OnClosed(ws, serviceProvider);
        }
    }

    private async Task<SalkyWebSocket?> TryAcceptConnection(HttpContext context,IServiceProvider provider)
    {
        if (!MakeHttpHandshake(context, provider, out var claims, out var key))
            return null;
        if (_connectionManager.TryGetSocket(key, out var removedSocket))
        {
            this.logger.LogInformation($"Attempt to connect, but the user is already logged, disconnecting the old connection and rejecting the actual..");
            await removedSocket.CloseOutputAsync(WebSocketCloseStatus.PolicyViolation, CloseDescription.DuplicatedConnection);
            return null;
        }
        return new SalkyWebSocket(await context.WebSockets.AcceptWebSocketAsync(), claims, key);
    }

  

    private async Task ReceiveMessage(SalkyWebSocket ws,IServiceProvider provider)
    {
        while (ws.State == WebSocketState.Open)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult? result = null;
            using var ms = new MemoryStream();
            bool canContinue = true;
            do
            {
                try
                {
                    result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                }
                catch(Exception ex)
                {
                    if (ws.State == WebSocketState.Aborted)
                    {
                        canContinue = false;
                        break;
                    }
                    else
                    {
                        this.logger.LogError(ex, "Unrecognized exception while receiving message.");
                        throw;
                    }
                }

                ms.Write(buffer.Array ?? throw new NullReferenceException(), buffer.Offset, result.Count);
            } while (!result.EndOfMessage);
            if (!canContinue) break;
            ms.Seek(0, SeekOrigin.Begin);
            await MessageRouter(ws, result?.MessageType ?? throw new NullReferenceException(nameof(result)), ms,provider);
        }
    }
    private async Task MessageRouter(SalkyWebSocket ws, WebSocketMessageType msgType, MemoryStream stream,IServiceProvider provider)
    {
        switch (msgType)
        {
            case WebSocketMessageType.Text:
                await OnTextReceived(ws, stream, provider);
                break;
            case WebSocketMessageType.Binary:
                await OnBinaryReceived(ws, stream, provider);
                break;
            case WebSocketMessageType.Close:
                await MessageCloseHandler(ws);
                break;
        }
    }

    public async Task OnBinaryReceived(SalkyWebSocket ws, MemoryStream stream, IServiceProvider provider)
    {
        await OnTextReceived(ws, stream, provider);
    }
    public async Task OnTextReceived(SalkyWebSocket ws, MemoryStream stream,IServiceProvider provider)
    {
        try
        {
            using var reader = new StreamReader(stream, Encoding.UTF8);
            var fullMessage = await reader.ReadToEndAsync();
            var msgResult = JsonSerializer.Deserialize<MessageServer>(fullMessage, DefaultJsonSerializerOptions);
            if (msgResult == null) throw new Exception("Message cannot be null");
            await provider.GetRequiredService<IRouterResolver>().Route(ws, msgResult,provider);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error while receiving message");
            await ws.SendErrorAsync("Error while process message", "error");
        }
    }

    public async Task OnClosed(SalkyWebSocket ws,IServiceProvider provider)
    {
        if (!this._connectionManager.TryRemoveSocket(ws.Key, out _))
        {
            this.logger.LogError("Connection not removed.");
        }
        await provider.GetRequiredService<IRouterResolver>().AfterClose(ws,provider);
        ws.Dispose();
    }
   
    public async Task AfterConnectionEstablished(SalkyWebSocket ws, IServiceProvider provider)
    {
        _connectionManager.AddSocket(ws.Key, ws);
        await provider.GetRequiredService<IRouterResolver>().AfterOpen(ws,provider);
    }
   
    internal async Task MessageCloseHandler(SalkyWebSocket webSocket)
    {
        if (webSocket.CanClose)
        {
            if (!Enum.TryParse<CloseDescription>(webSocket.CloseStatusDescription, true, out var parsedDescription))
                parsedDescription = CloseDescription.Unknow;
            await webSocket.CloseOutputAsync(webSocket.CloseStatus ?? WebSocketCloseStatus.Empty, parsedDescription);
        }
    }

}
