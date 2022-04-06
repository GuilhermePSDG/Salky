using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebSocket.Shared.DataAcess.Models
{
    public class UsuarioVM : BaseEntityVM
    {
        public byte[] PrivateKey { get; set; }
        public bool VisivelUltimoLogin { get; set; } = false;
        public ObservableCollection<ContatoVM> Contatos { get; set; } = new();

        public string ExibitionName { get; set; }
        public byte[] PublicKey { get; set; }
        public string PictureSource { get; set; }
        public string NickColor { get; set; } = "#FFFFFF";
        public string Status => VisivelUltimoLogin ? "Visivel" : "Invisivel";

    }
}
