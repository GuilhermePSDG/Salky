using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Wpf.Core.Models;
using System.IO;
using WebSocket.Shared.DataAcess.Models;

namespace Wpf.MVVM.Models
{
    public class UsuarioVM : BaseEntityVM
    {
        public UsuarioVM(Usuario x)
        {
            throw new NotImplementedException();
        }
        

        public string ExibitionName { get; set; }
        public string PublicKey { get; set; }
        public string PictureSource { get; set; }
        public string NickColor { get; set; } = "#FFFFFF";
        public string? WebSocketConnectionID { get; set; }
        public string Status 
        { 
            get => EstaVisivel ? "Visivel" : "Invisivel"; 
            set => EstaVisivel = value == "Visivel" ? true : false; 
        }
        public bool EstaVisivel { get; set; } = false;

        public string PrivateKey { get; set; }

        public ObservableCollection<ContatoVM> Contatos { get; set; } = new();

    }

}
