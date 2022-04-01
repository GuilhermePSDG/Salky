using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared;
using WebSocket.Shared.DataAcess.Local.Services;
using WebSocket.Shared.DataAcess.Models;
using Wpf.Core;
using Wpf.Core.Models;
using Wpf.MVVM.Models;
using Wpf.MVVM.Views;

namespace Wpf.MVVM.ViewModels
{
    public class LoginViewModel : BaseEntityVM
    {
        public string AnonymousImageSource = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTTAR6tkzT15xcnDWO3rJ88fsDrZMg07uuuYjvLwMbC3nZoRU3qpJFcWLTfTsL5ZPkydkQ&usqp=CAU";
        public Action? Close;
        private IRepository<Usuario> userRepo;
        private ObservableCollection<UsuarioVM> Accounts { get; set; }
        public UsuarioVM? SelectedAccount { get; set; }
        //
        private Command RegisterCommand { get; set; }
        private Command LoginCommand { get; set; }
        //
        private string RegisterName { get; set; } = "";
        
        public LoginViewModel(IRepository<Usuario> userRepository)
        {
            this.userRepo = userRepository;
            var users = userRepo.GetAll() ?? new();
            this.Accounts = new ObservableCollection<UsuarioVM>(users.Select(x => new UsuarioVM(x)));
            this.RegisterCommand = new Command(Register,() => !string.IsNullOrEmpty(RegisterName) && RegisterName.Length > 3);
            this.LoginCommand = new Command(Login, () => SelectedAccount != null);
        }
        public void Login()
        {
            if (Close == null) throw new Exception("LoginViewModel.Close cannot be null");
            Close();
        }
        public async void Register()
        {
            var taskMesageBox = Task.Run(()=>new MessageBoxC($"Ola {RegisterName}, sua chave privada está sendo gerada, aguarde..").ShowDialog());
            var rsaService = await Task.Run(()=> RsaService.GenereteNewKeys());
            var usuario = new Usuario()
            {
                ExibitionName = RegisterName,
                PictureSource = AnonymousImageSource,
                PrivateKey = rsaService.GetPrivateKey(),
                PublicKey = rsaService.GetPublicKey(),
                VisivelUltimoLogin = false,
            };
            this.userRepo.Add(usuario);
            await this.userRepo.SaveChangesAsync();
            await taskMesageBox;
        }
    }
}
