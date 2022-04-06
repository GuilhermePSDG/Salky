using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WebSocket.Shared.DataAcess.Models
{
    public class ContatoVM : BaseEntityVM
    {
        public ObservableCollection<MessageVM> Messages { get; set; } = new();

        public string ExibitionName { get; set; }
        public byte[] PublicKey { get; set; }
        public string PictureSource { get; set; }
        public string NickColor { get; set; } = "#FFFFFF";
        public string LastMessage => Messages.LastOrDefault()?.Content ?? "";

        public bool IsSync { get; internal set; } = false;
    }

}
