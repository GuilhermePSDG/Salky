
namespace Salky.WebSocket.Infra.Models;

public class MessageServer
{
    public MessageServer()
    {
        CreatedAt = DateTime.UtcNow.Ticks;
    }

    public MessageServer(object data, string path, Method method) : this(path,method)
    {
        Data = data;
    }

    public MessageServer(string path, Method method) : this()
    {
        Path = path;
        Method = method;
    }

    public object? Data { get; set; }
    public long CreatedAt { get; set; }
    public string Path { get; set; }
    public Method Method { get; set; }
}
