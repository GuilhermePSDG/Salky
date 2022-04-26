using Newtonsoft.Json;
using Salky.App.Dtos.Users;
using Salky.WebSocket.Infra.Models;
using System.Net.WebSockets;
using System.Text;

namespace Salky.WebSocket.Infra.Socket;

public class SalkyWebSocket 
{
    public event EventHandler<MessageServer> OnMessageReceived = (e, f) => { };
    public void Invoke(MessageServer messageServer) => OnMessageReceived.Invoke(null, messageServer);
    public Storage Storage { get; } = new Storage();
    //
    public Guid UniqueId = Guid.NewGuid();
    private System.Net.WebSockets.WebSocket webSocket { get; set; }
    public UserDto user { get; set; }
    public SalkyWebSocket(System.Net.WebSockets.WebSocket webSocket, UserDto user)
    {
        if (user == null)
        {
            var msg = $"The parrameter {nameof(SalkyWebSocket.user)} cannot be null or invalid";
            webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, msg, CancellationToken.None);
            throw new Exception(msg);
        }
        this.webSocket = webSocket;
        this.user = user;
    }
    public string? CloseStatusDescription => webSocket.CloseStatusDescription;
    public WebSocketCloseStatus? CloseStatus => webSocket.CloseStatus;
    public WebSocketState State => webSocket.State;
    public void Abort() => webSocket.Abort();
    public async Task CloseRequest(WebSocketCloseStatus closeStatus, string? statusDescription) =>  await webSocket.CloseAsync(closeStatus, statusDescription, CancellationToken.None);
    public async Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription) => await webSocket.CloseOutputAsync(closeStatus, statusDescription, CancellationToken.None);
    public void Dispose()
    {   
        webSocket.Abort();
        webSocket.Dispose();
        webSocket = null;
        Storage.Dispose();
        user = null;
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return await webSocket.ReceiveAsync(buffer, cancellationToken);
    }

    public async Task SendMessageServer(MessageServer messageServer)
    {
        var json = JsonConvert.SerializeObject(messageServer);
        var buffer = Encoding.UTF8.GetBytes(json);
        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None); ;
    }
    public async Task SendAsync<T>(T data, string path, Method method)
    {
        var msgServer = new MessageServer()
        {
            DataJson = JsonConvert.SerializeObject(data),
            MethodEnum = method,
            PathString = path,
            SenderIntentifier = user.Id.ToString(),
        };
        await SendMessageServer(msgServer);
    }

    public async Task SendErrorAsync(string message,Exception exception)
    {
        var errorMsg = new MessageServer()
        {
            CreatedAt = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            DataJson = JsonConvert.SerializeObject(new
            {
                message = message,
                error = exception.Message,
            }),
            MethodEnum = Method.POST,
            PathString = "error",
            SenderIntentifier = user.UserName,
        };
        await SendMessageServer(errorMsg);
    }


}
