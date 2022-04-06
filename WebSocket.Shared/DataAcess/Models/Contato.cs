using System.Collections.Generic;

namespace WebSocket.Shared.DataAcess.Models
{
    public class Contato : BaseEntity
    {
        public List<Message> Messages { get; set; } = new();

        public string ExibitionName { get; set; }
        public byte[] PublicKey { get; set; }
        public string PictureSource { get; set; }
        public string NickColor { get; set; } = "#FFFFFF";
    }

}
