namespace WebSocket.Shared.Models;

public class MessageServer
{
    public MessageServer() { }
    public MessageServer(string receiver, string sender, string PathString, string json)
    {
        this.Receiver = receiver;
        this.Sender = sender;
        this.PathString = PathString;
        this.Json = json;
    }

    public string Receiver { get; set; }
    public string Sender { get; set; }
    public string[] PathArray => PathString.Split('/');
    public string PathString { get; set; }
    public string Json { get; set; }
}