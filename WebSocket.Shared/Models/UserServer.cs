using System.Text.Json.Serialization;

namespace WebSocket.Shared.Models;

public class UserServer
{
    public UserServer(string Apelido, bool IsVisible, byte[] PublicKey)
    {
        this.Apelido = Apelido;
        this.IsVisible = IsVisible;
        this.PublicKey = PublicKey;
    }
    public string Apelido { get; set; }
    public bool IsVisible { get; set; }
    public byte[] PublicKey { get; set; }


    public bool IsValid()
    {
        return Apelido != null && PublicKey != null && Apelido.Length > 3;
    }


}
