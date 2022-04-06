using System;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using WebSocket.Shared;
using WebSocket.Shared.DataAcess;
using WebSocket.Shared.Models;
using WebSocket.Shared.DataAcess.Models;
using Wpf.Core;
using Wpf.MVVM.Views;
using Wpf.Core.Models;
using System.Windows;
using System.Text;
using WebSocket.Shared.DataAcess.Local;
using Microsoft.EntityFrameworkCore;
using WebSocket.Shared.DataAcess.Local.Repositories;
using Wpf.Services;
using System.Net.Http;

namespace Wpf.MVVM.ViewModels
{
    public class MainViewModel : BaseEntityVM , IDisposable
    {
        private UserService userService;
        private Login loginWindow;
        private AudioAdapter audioAdapter;

        public ICommand SendCommand { get; set; }
        public ICommand ChangeVisibilityCommand { get; set; }
        public ICommand ConfigCommand { get; set; }
        public ICommand LoginUserCommand { get; set; }
        public ICommand SendFileCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public UsuarioVM LoggedUserVM { get; set; }
        public ContatoVM? SelectedContact { get; set; }
        public string SearchUserText { get; set; }
        public string MessageToSend { get; set; } = string.Empty;
        public SalkyWebSocketClient SalkyWebSocket { get; set; }

        private const string SALKY_WEB_URL = "ws://localhost:5281"; // "ws://salky-websocket.herokuapp.com";
        private record Audio(byte[] buffer, int lenght);
        public MainViewModel(Login loginWindow, UserService userService)
        {
            this.userService = userService;
            this.loginWindow = loginWindow;
            this.audioAdapter = new AudioAdapter();
            this.audioAdapter.StartMicrofoneListener();
            this.audioAdapter.StartHeadPhoneListener();
            this.audioAdapter.AddMicrofoneAudioHandler(x => this.audioAdapter.ReproduceAudio(x.Buffer, x.BytesRecorded));
            MakeLogin();
            userService.RefreshPictureContato(LoggedUserVM.Id, LoggedUserVM.Contatos.First().Id, new HttpClient().GetByteArrayAsync(LoggedUserVM.Contatos.First().PictureSource).Result);
            StartSocketClient().ContinueWith(async x =>
            {
                this.SalkyWebSocket = await x;
                StartRoutes();
                StartCommands();
            });
        }
        private async Task<SalkyWebSocketClient> StartSocketClient()
        {
            return await SalkyWebSocketClient.StartNewAsync
                (() => new UserServer(LoggedUserVM.ExibitionName,LoggedUserVM.VisivelUltimoLogin,LoggedUserVM.PublicKey),
                q =>this.LoggedUserVM.ExibitionName = q.Apelido,
                this.LoggedUserVM.PrivateKey
                ,new Uri(SALKY_WEB_URL));
        }


        private void StartRoutes()
        {
            SalkyWebSocket.OnPost<ContatoVM>((con,msg) => ReceberContato(con, msg));
            SalkyWebSocket.OnPost<MessageVM>((msg,msgserver) => ReceberMensagem(msg, msgserver.SenderPublicKey,false));
            //
            //Não vai enviar a chave publica, deve ser recibida via msgserver
            //Não vai enviar porque futuramente vai ficar cryptografado com RSA
            //Que não é capaz de cryptografar arquivos grandes
            SalkyWebSocket.OnGet<ContatoVM>(() =>
            {
                return new ContatoVM()
                {
                    ExibitionName = this.LoggedUserVM.ExibitionName,
                    NickColor = this.LoggedUserVM.NickColor,
                    PictureSource = this.LoggedUserVM.PictureSource,
                };
            });

            SalkyWebSocket.OnRoute("route/get/sync", async (msg) =>
            {
                var me = new ContatoVM()
                {
                    ExibitionName = this.LoggedUserVM.ExibitionName,
                    NickColor = this.LoggedUserVM.NickColor,
                    PictureSource = this.LoggedUserVM.PictureSource,
                    PublicKey = this.LoggedUserVM.PublicKey,
                };
                var message = new MessageServer(msg.SenderPublicKey,msg.ReceiverPublicKey,"route/set/sync",JsonSerializer.SerializeToUtf8Bytes(me));
                await SalkyWebSocket.SendMessageServer(message);
            });

            SalkyWebSocket.OnRoute("route/set/sync", (msg) =>
            {
                var contatoVM = JsonSerializer.Deserialize<ContatoVM>(msg.Data);
                SyncConcatData(contatoVM, msg);
            });

            SalkyWebSocket.OnRoute("error", q => new MessageBoxC(Encoding.UTF8.GetString(q.Data)).ShowDialog());
        }


        private void MakeLogin()
        {
            loginWindow.ShowDialog();
            this.LoggedUserVM = loginWindow.GetLoggedUserVM();
            loginWindow.Hide();
            foreach(var contato in LoggedUserVM.Contatos)
                for(int i = 1;i < contato.Messages.Count; i++)
                    if(contato.Messages[i - 1].IsOwner == contato.Messages[i].IsOwner)
                        contato.Messages[i].IsSequencialMessage = true;
        }

        private void StartCommands()
        {
            this.SendCommand = new Command(SendMessage, (x) => MessageToSend != null && MessageToSend.Length > 0 && SalkyWebSocket != null && SalkyWebSocket.IsConnected && this.LoggedUserVM != null);
            this.ChangeVisibilityCommand = new Command(ChangeVisibility, (x) => SalkyWebSocket != null && SalkyWebSocket.IsConnected && this.LoggedUserVM != null);
            this.ConfigCommand = new CommandLock(StartNewConfigView);
            this.SearchCommand = new CommandLock(BuscarUsuario, (x) => SearchUserText != null && SearchUserText.Length > 0 && SalkyWebSocket != null && SalkyWebSocket.IsConnected && this.LoggedUserVM != null);
            this.LoginUserCommand = new CommandLock(MakeLogin);
        }
        private void ReceberMensagem(MessageVM msg, byte[] ContatoPublicKey,bool IsOwner)
        {
            msg.IsOwner = IsOwner;
            userService.AddMessage(this.LoggedUserVM.Id,ContatoPublicKey,msg);
            var index = this.LoggedUserVM.Contatos.ToList().FindIndex(x => x.PublicKey.SequenceEqual(ContatoPublicKey));
            if(index != -1)
            {
                if (!this.LoggedUserVM.Contatos[index].IsSync)
                {
                    this.LoggedUserVM.Contatos[index].IsSync = true;
                    SendGetSync(ContatoPublicKey);
                }
                this.LoggedUserVM.Contatos[index].Messages.Add(msg);
                this.SelectedContact = this.LoggedUserVM.Contatos[index];
            }

            ////Deve adicionar a mensagem no contato caso ele exista
            //var contato = this.LoggedUserVM.Contatos
            //    .FirstOrDefault(x => x.PublicKey.SequenceEqual(ContatoPublicKey));
            ////Caso não exista, deve pedir o contato,
            //if(contato == null)
            //{
            //    await this.SalkyWebSocket.SendGet<ContatoVM>(ContatoPublicKey);
            //}
            //else
            //{
            //    using (var userRepo = App.RepositoryFactory.GetRepository<Usuario>())
            //    {
            //        var usuario = userRepo.GetById(this.LoggedUserVM.Id);
            //        if (usuario == null)
            //            throw new Exception("Usuario logado não existe no banco de dados");
            //        var contato_db = usuario.Contatos.FirstOrDefault(x => x.PublicKey.SequenceEqual(ContatoPublicKey));
            //        if (contato_db == null)
            //            throw new Exception("Contato não existe no banco de dados");
            //        var messageToAdd = Mapping.Map<Message>(msg);
            //        messageToAdd.IsOwner = IsOwner;
            //        contato_db.Messages.Add(messageToAdd);
            //        userRepo.Update(usuario);
            //        var savedCount = userRepo.SaveChanges();
            //        if (savedCount == 0)
            //            throw new Exception("Não foi possivel adicionar o usuario no contatos");
            //        //Seta novamente o objeto para obter as novos valores
            //        msg = Mapping.Map<MessageVM>(messageToAdd);
            //    }
            //    contato.Messages.Add(msg);
            //    if (!contato.IsSync)
            //    {
            //        contato.IsSync = true;
            //        SendGetSync(contato.PublicKey);
            //    }
            //}

        }

        private async void SendMessage(object? obj)
        {
            //Deve enviar uma mensagem para o usario selecionado
            var msg = new MessageVM()
            {
                Content = this.MessageToSend,
                Date = DateTime.Now,
            };
            await this.SalkyWebSocket.SendPost<MessageVM>(msg, 
            SelectedContact.PublicKey);
            ReceberMensagem(msg, SelectedContact.PublicKey, true);
            this.MessageToSend = "";

        }

        private void ReceberContato(ContatoVM contato, MessageServer msgServer)
        {
            contato.PublicKey = msgServer.SenderPublicKey;
            userService.AddContato(LoggedUserVM.Id, contato);
            this.LoggedUserVM.Contatos.Add(contato);
            this.SelectedContact = contato;


            //Deve adicionar o contato caso ele não exista
            //var contatoVM = this.LoggedUserVM.Contatos.FirstOrDefault(x => x.PublicKey.Equals(msgServer.SenderPublicKey));
            //Se não possui o usuario
            //if (!LoggedUserVM.Contatos.Any(x => x.PublicKey.Equals(msgServer.SenderPublicKey)))
            //{
            //    Pergunta se quer adicionar
            //    new MessageBoxC($"Novo usuario, deseja adicionar o {contato.ExibitionName} ? ", () =>
            //    {
            //        Seta a chave publica
            //        contato.PublicKey = msgServer.SenderPublicKey;
            //        Adicionar no banco de dados
            //        using (var userRepo = App.RepositoryFactory.GetRepository<Usuario>())
            //        {
            //            var usuario = userRepo.GetById(this.LoggedUserVM.Id);
            //            if (usuario == null)
            //                throw new Exception("Usuario logado não existe no banco de dados");
            //            var contatoToAdd = Mapping.Map<Contato>(contato);
            //            usuario.Contatos.Add(contatoToAdd);
            //            userRepo.Update(usuario);
            //            var savedCount = userRepo.SaveChanges();
            //            if (savedCount == 0)
            //                throw new Exception("Não foi possivel adicionar o usuario no contatos");
            //            Seta novamente o objeto para obter as novos valores
            //            contato = Mapping.Map<ContatoVM>(contatoToAdd);
            //        }
            //        this.LoggedUserVM.Contatos.Add(contato);
            //    }, () => { }).ShowDialog();
            //}
        }

        private async void BuscarUsuario(object? obj)
        {
            var user = await this.SalkyWebSocket.GetUser(this.SearchUserText);
            if(user == null)
                new MessageBoxC($"Nenhum resultado para {this.SearchUserText}").ShowDialog();
            else
                await this.SalkyWebSocket.SendGet<ContatoVM>(user.PublicKey);
            this.SearchUserText = "";
        }

        private void StartNewConfigView(object? obj)
        {
            //Deve inicializar a tela de login
        }

        private void ChangeVisibility(object? obj)
        {
            //Deve mudar a visibilidade do usuario no servidor
        }

        public async void SendGetSync(byte[] TargetPublicKey)
        {
            var message = new MessageServer(TargetPublicKey,LoggedUserVM.PublicKey,"route/get/sync",new byte[0]);
            await SalkyWebSocket.SendMessageServer(message);
        }

        public void SyncConcatData(ContatoVM contato, MessageServer msg)
        {
            contato.PublicKey = msg.SenderPublicKey;
            userService.UpdateContato(LoggedUserVM.Id, msg.SenderPublicKey, contato);
            var index = this.LoggedUserVM.Contatos.ToList().FindIndex(x => x.PublicKey.SequenceEqual(contato.PublicKey));
            if (index != -1)
            {
                this.LoggedUserVM.Contatos[index] = userService.GetContatoByPK(LoggedUserVM.Id, contato.PublicKey) ?? contato;
                this.SelectedContact = this.LoggedUserVM.Contatos[index];
            }
            //var contatoVM_Index = this.LoggedUserVM.Contatos.ToList().FindIndex(x => x.PublicKey.SequenceEqual(msg.SenderPublicKey));
            //using(var userRepo = App.RepositoryFactory.GetRepository<Usuario>())
            //{
            //    var logged_user = userRepo.GetById(LoggedUserVM.Id);
            //    var contato_db_index= logged_user.Contatos.FindIndex(x => x.PublicKey.SequenceEqual(msg.SenderPublicKey));
            //    contato.Id = logged_user.Contatos[contato_db_index].Id;
            //    logged_user.Contatos[contato_db_index] = Mapping.Map<Contato>(contato);
            //    userRepo.Update(logged_user);
            //    var n = userRepo.SaveChanges();
            //    this.LoggedUserVM.Contatos[contatoVM_Index] = Mapping.Map<ContatoVM>(logged_user.Contatos[contato_db_index]);
            //}
        }

        public void Dispose()
        {
            loginWindow = null;
            audioAdapter = null;
            SendCommand = null;
            ChangeVisibilityCommand = null;
            ConfigCommand = null;
            LoginUserCommand = null;
            SendFileCommand = null;
            SearchCommand = null;
            LoggedUserVM = null;
            SelectedContact = null;
            SearchUserText = null;
            MessageToSend = null;
            SalkyWebSocket.Dispose();
        }
    }
}
