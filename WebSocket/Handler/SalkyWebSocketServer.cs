using System.Net.WebSockets;
using WebSocket.Shared.Models;

namespace WebSocket.Handler;

public class SalkyWebSocketServer : System.Net.WebSockets.WebSocket
{
    private System.Net.WebSockets.WebSocket webSocket { get; set; }
    public UserServer? user { get; set; }
    public string Id { get; internal set; }

    public SalkyWebSocketServer(System.Net.WebSockets.WebSocket webSocket)
    {
        this.webSocket = webSocket;
        this.Id = Guid.NewGuid().ToString();
    }

    public override WebSocketCloseStatus? CloseStatus => webSocket.CloseStatus;

    public override string? CloseStatusDescription => webSocket.CloseStatusDescription;

    public override WebSocketState State => webSocket.State;

    public override string? SubProtocol => webSocket.SubProtocol;

    public override void Abort()
    {
        webSocket.Abort();
    }
    public override Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken)
    {
        return webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }
    public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken)
    {
        return webSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
    }
    public override void Dispose()
    {
        webSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData,"Server WebSocket Disposed",CancellationToken.None).Wait();
        webSocket.Dispose();
        webSocket = null;
        Id = null;
        user = null;
    }

    public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return webSocket.ReceiveAsync(buffer, cancellationToken);
    }

    public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        return webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }

}
