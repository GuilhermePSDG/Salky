
namespace Salky.WebSocket.Infra.Models;

public class MessageServer: RoutePathBase
{
    public MessageServer()  { }
    public MessageServer(string path, Method method, Status status) : base(path,method)
    {
        CreatedAt = DateTime.UtcNow.Ticks;
        Status = status;
    }
    public MessageServer(string path, Method method,Status status, object? data) : this(path,method,status)
    {
        Data = data;
    }
    public Status Status { get; set; }
    public object? Data { get; set;}
    public long CreatedAt { get; set;}
}
