using System.Collections.Generic;

namespace WebSocket.Shared.DataAcess.Models
{
    public class Contato : UsuarioMinimo
    {
        public List<Message> Messages { get; set; } = new();
    }

}
