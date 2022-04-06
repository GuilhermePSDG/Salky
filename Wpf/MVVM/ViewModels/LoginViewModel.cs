using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Shared;
using WebSocket.Shared.DataAcess;
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
        public ObservableCollection<UsuarioVM> Accounts { get; set; }
        public UsuarioVM? SelectedAccount { get; set; }
        //
        public Command RegisterCommand { get; set; }
        public Command LoginCommand { get; set; }
        //
        public string RegisterName { get; set; } = "";
        
        public LoginViewModel()
        {
            using(var repo = App.RepositoryFactory.GetRepository<Usuario>().AsNoTracking())
                this.Accounts = new ObservableCollection<UsuarioVM>((repo.GetAll() ?? new()).Select(x => Mapping.Map<UsuarioVM>(x)));
            this.RegisterCommand = new Command(Register,() => this.RegisterName.Length > 3);
            this.LoginCommand = new Command(Login, () => SelectedAccount != null);
        }
        public void Login()
        {
            App.Current.MainWindow.Hide();
        }
        public async void Register()
        {
            new MessageBoxC($"Ola {RegisterName}, sua chave privada está sendo gerada, aguarde..").Show();
            var rsaService = await Task.Run(()=> RsaService.GenereteNewKeys());
            var user = await Task.Run(() =>
            {
                return new Usuario()
                {
                    ExibitionName = RegisterName,
                    PictureSource = AnonymousImageSource,
                    PrivateKey = rsaService.GetPrivateKey(),
                    PublicKey = rsaService.GetPublicKey(),
                    VisivelUltimoLogin = false,
                };
            });

            using (var userRepo = App.RepositoryFactory.GetRepository<Usuario>())
                userRepo.Add(user);

            var tempuservm = Mapping.Map<UsuarioVM>(user);
            this.Accounts.Add(tempuservm);
            this.SelectedAccount = tempuservm;
            rsaService.Dispose();
            rsaService = null;
            new MessageBoxC($"Ola {RegisterName}, sua conta foi gerada.").ShowDialog();
            App.Current.MainWindow.Hide();
        }

    }
}
