using System.Text.Json;
using System.Text.Json.Serialization;
using Salky.WebSocket.Infra.Models;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Salky.WebSocket.Infra.Socket;


public class SalkyWebSocket
{
    public event EventHandler<MessageServer> OnMessageReceived = (e, f) => { };
    public DateTime CreatedAt { get; }
    public void InvokeMessageReceived(MessageServer messageServer) => OnMessageReceived.Invoke(null, messageServer);
    public ConcurrentStorage Storage { get; } = new ConcurrentStorage();
    //
    public Guid UniqueId = Guid.NewGuid();
    private System.Net.WebSockets.WebSocket webSocket { get; set; }
    public List<Claim> UserClaims { get; set; }

    public SalkyWebSocket(System.Net.WebSockets.WebSocket webSocket, List<Claim> UserClaims)
    {
        this.CreatedAt = DateTime.UtcNow;
        this.webSocket = webSocket;
        this.UserClaims = UserClaims;
    }



    public string? CloseStatusDescription => webSocket.CloseStatusDescription;
    public WebSocketCloseStatus? CloseStatus => webSocket.CloseStatus;
    public WebSocketState State => webSocket.State;
    public bool ConnectionsIsOpen => State == WebSocketState.Open;
    
    public string? SubProtocol => throw new NotImplementedException();
    public void Abort() => webSocket.Abort();
    //public async Task CloseAsync(WebSocketCloseStatus closeStatus, CloseDescription statusDescription) =>  await webSocket.CloseAsync(closeStatus, statusDescription.ToString(), CancellationToken.None);

    public async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, CloseDescription statusDescription) => 
        await webSocket.CloseOutputAsync(closeStatus, statusDescription.ToString(), CancellationToken.None);
   
    public bool Disposed = false;
    public void Dispose()
    {
        Disposed = true;
        TRY(webSocket.Dispose);
        TRY(Storage.Dispose);
        TRY(UserClaims.Clear);
    }
    private void TRY(Action act)
    {
        try{act();}finally{}
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return await webSocket.ReceiveAsync(buffer, cancellationToken);
    }

    public async Task SendMessageServer(MessageServer messageServer)
    {
        var json = JsonSerializer.Serialize(messageServer, DefaultJsonSerializerOptions);
        var buffer = Encoding.UTF8.GetBytes(json);
        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None); ;
    }


    /// <summary>
    /// Send error in <see cref="MessageServer.Path"/> = "error" and <see cref="MessageServer.Method"/> = "<see cref="Method.POST"/>"
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task SendErrorAsync(string message,string path,Method method = Method.POST)
    {
        await SendMessageServer(new MessageServer(path, method, Status.Error, message));
    }

}
