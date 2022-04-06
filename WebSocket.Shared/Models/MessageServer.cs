namespace WebSocket.Shared.Models;

public class MessageServer
{
    public MessageServer(byte[] ReceiverPublicKey, byte[] SenderPublicKey, string PathString, byte[] data)
    {
        this.ReceiverPublicKey = ReceiverPublicKey;
        this.SenderPublicKey = SenderPublicKey;
        this.PathString = PathString;
        this.Data = data;
    }

    public byte[] ReceiverPublicKey { get; set; }
    public byte[] SenderPublicKey { get; set; }
    public string[] PathArray => PathString.Split('/');
    public string PathString { get; set; }
    public byte[] Data { get; set; }
}