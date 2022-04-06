using System.Collections.Generic;

namespace WebSocket.Shared.DataAcess.Models
{
    public class Usuario : BaseEntity
    {
        public byte[] PrivateKey { get; set; }
        public bool VisivelUltimoLogin { get; set; } = false;
        public List<Contato> Contatos { get; set; } = new();

        public string ExibitionName { get; set; }
        public byte[] PublicKey { get; set; }
        public string PictureSource { get; set; }
        public string NickColor { get; set; } = "#FFFFFF";


    }
}
