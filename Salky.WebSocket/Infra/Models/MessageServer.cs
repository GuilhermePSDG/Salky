
namespace Salky.WebSocket.Infra.Models;

public class MessageServer
{
    public MessageServer()
    {
        CreatedAt = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }
    public string? SenderIntentifier { get; set; }
    public string? DataJson { get; set; }
    public string Method
    {
        get => MethodEnum.ToString();
        set => MethodEnum = (Method)Enum.Parse(typeof(Method), value.ToUpper());
    }
    [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
    public Method MethodEnum { get; set; }
    [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
    public string[] PathArray => PathString.Split('/');
    public string PathString { get; set; }
    public long CreatedAt { get; set; }
}
