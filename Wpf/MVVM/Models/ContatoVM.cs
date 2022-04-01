using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using WebSocket.Shared.Models;
using Wpf.Core.Models;

namespace Wpf.MVVM.Models
{
    public class ContatoVM : BaseEntityVM
    {
        public ContatoVM()
        {

        }
        public ContatoVM(UsuarioVM vm)
        {
            this.ExibitionName = vm.ExibitionName;
            this.PublicKey = vm.PublicKey;
            this.PictureSource = vm.PictureSource;
            this.NameColor = vm.NickColor;
        }
        public ContatoVM(UserServer vm)
        {
            var contato = JsonSerializer.Deserialize<ContatoVM>(vm.PlusData);
            this.ExibitionName = contato.ExibitionName;
            this.PublicKey = contato.PublicKey;
            this.PictureSource = contato.PictureSource;
            this.NameColor = contato.NameColor;
        }

        public ContatoVM(MessageVM message)
        {
            this.ExibitionName = message.SenderName;
            this.PublicKey = message.SenderPublicKey;
            this.NameColor = message.UsernameColor;
            this.PictureSource = message.ImageSource;
        }


        public string ExibitionName { get; set; }
        public string PublicKey { get; set; }
        public string PictureSource { get; set; }
        public string NameColor { get; set; }
        //
        public ObservableCollection<MessageVM> Messages { get; set; } = new();
        public string LastMessage => Messages.Count > 0 ? Messages.Last().Content : "";
    }

}
