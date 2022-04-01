using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Shared.DataAcess.Models
{
    public class UsuarioMinimo : BaseEntity
    {
        public UsuarioMinimo() { }
        public UsuarioMinimo(string exibitionName, string publicKey, string pictureSource, string nickColor)
        {
            ExibitionName = exibitionName;
            PublicKey = publicKey;
            PictureSource = pictureSource;
            NickColor = nickColor;
        }


        public string ExibitionName { get; set; }
        public string PublicKey { get; set; }
        public string PictureSource { get; set; }
        public string NickColor { get; set; } = "#FFFFFF";

    }
}
