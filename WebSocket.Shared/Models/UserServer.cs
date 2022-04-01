using System.Text.Json.Serialization;

namespace WebSocket.Shared.Models;

public class UserServer
{
    public UserServer() { }
    public UserServer(string Apelido, bool IsVisible ,string PlusData)
    {
        this.Apelido = Apelido;
        this.IsVisible = IsVisible;
        this.PlusData = PlusData;
    }
    public string Apelido { get; set; }
    public bool IsVisible { get; set; }
    public string PlusData { get; set; }
    public string? ConnectionKey { get; set; }

}
