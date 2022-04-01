using System.Collections.Generic;

namespace WebSocket.Shared.DataAcess.Models
{
    public class Usuario : UsuarioMinimo
    {
        public string PrivateKey { get; set; }
        public bool VisivelUltimoLogin { get; set; } = false;
        public List<Contato> Contatos { get; set; } = new();
    }
}
