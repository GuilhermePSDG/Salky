
namespace Salky.WebSocket.Infra.Models;

public class MessageServer
{
    public MessageServer()
    {
        CreatedAt = DateTime.UtcNow.Ticks;
    }
    public MessageServer(string path, Method method, Status status) : this()
    {
        Path = path;
        Method = method;
        Status = status;
    }

    public MessageServer(string path, Method method,Status status, object? data) : this(path,method,status)
    {
        Data = data;
    }

    public string Path { get; set; }
    public Method Method { get; set;}
    public Status Status { get; set; }
    public object? Data { get; set;}
    public long CreatedAt { get; set;}

}
