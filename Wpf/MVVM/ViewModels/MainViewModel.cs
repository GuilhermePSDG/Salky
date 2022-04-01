using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WebSocket.Shared;
using WebSocket.Shared.Models;
using Wpf.Core;
using Wpf.Core.Models;
using Wpf.MVVM.Models;
using Wpf.MVVM.Views;

namespace Wpf.MVVM.ViewModels
{
    public class MainViewModel : BaseEntityVM
    {
        public string AnonymousImageSource = "https://cdn-icons-png.flaticon.com/512/634/634741.png";
        private AudioAdapter audioAdapter;

        public Command SendCommand { get; set; }
        public Command ChangeVisibilityCommand { get; set; }
        public Command ConfigCommand { get; set; }
        public Command LoginUserCommand { get; set; }
        public Command SendFileCommand { get; }


        public UsuarioVM LoggedUser { get; set; }
        public ContatoVM? SelectedContact { get; set; }
        public string SearchUserText { get; set; }
        public Command SearchCommand { get; set; }
        public string MessageToSend { get; set; } = string.Empty;
        public SalkyWebSocketClient SalkyWebSocket { get; set; }
        private const string SALKY_WEB_URL = "ws://localhost:5281";//"ws://salky-websocket.herokuapp.com"; 
        private record Audio(byte[] buffer, int lenght);
        private async Task<SalkyWebSocketClient> StartSocketClient()
        {
            return await SalkyWebSocketClient.StartNewAsync
                 (
                     () => new WebSocket.Shared.Models.UserServer
                     {
                         Apelido = this.LoggedUser.ExibitionName,
                         ConnectionKey = this.LoggedUser.WebSocketConnectionID,
                         IsVisible = this.LoggedUser.EstaVisivel,
                         PlusData = JsonSerializer.Serialize(new ContatoVM(this.LoggedUser))
                     },
                     q =>
                     {
                         this.LoggedUser.EstaVisivel = q.IsVisible;
                         this.LoggedUser.WebSocketConnectionID = q.ConnectionKey;
                         this.LoggedUser.ExibitionName = q.Apelido;
                     },
                     new Uri(SALKY_WEB_URL)
                 );
        }
        private bool IsSendingFile { get; set; } = false;
        public MainViewModel()
        {
            this.audioAdapter = new AudioAdapter();
            this.audioAdapter.StartMicrofoneListener();
            this.audioAdapter.StartHeadPhoneListener();
            this.audioAdapter.SelfListerner();
            MakeLogin();
            StartSocketClient().ContinueWith(async x =>
            {
                this.SalkyWebSocket = await x;
                StartRoutes();

            });
            StartCommands();
            StartSocketClient();
        }

        private Action<Task, object?> StartRoutes()
        {
            //Iniciar as rotas client - client
            SalkyWebSocket.On("route/message", (message) =>
            {
                if (message != null)
                {
                    var receivedMessageVM = JsonSerializer.Deserialize<MessageVM>(message.Json);
                    if (receivedMessageVM != null)
                        ReceiveMessage(receivedMessageVM);
                }
            });
            SalkyWebSocket.On("route/audio", (msg) =>
            {
                var audio = JsonSerializer.Deserialize<Audio>(msg.Json);
                this.audioAdapter.ReproduceAudio(audio.buffer, audio.lenght);
            });
        }

        private void MakeLogin()
        {
            throw new NotImplementedException();
        }

        private void StartCommands()
        {
            this.SendCommand = new Command(SendMessage, () => MessageToSend != null && MessageToSend.Length > 0 && SalkyWebSocket != null && SalkyWebSocket.IsConnected && this.LoggedUser != null);
            this.ChangeVisibilityCommand = new Command(ChangeVisibility, () => SalkyWebSocket != null && SalkyWebSocket.IsConnected && this.LoggedUser != null);
            this.ConfigCommand = new Command(StartNewConfigView);
            this.SearchCommand = new Command(BuscarUsuario, () => SearchUserText != null && SearchUserText.Length > 0 && SalkyWebSocket != null && SalkyWebSocket.IsConnected && this.LoggedUser != null);
            this.LoginUserCommand = new Command(StartSocketClient);
        }




        //Depois do HandShake, caso o objeto volte assinado é porque o destinatario aceitou a transferencia
        //A assinatura será um texto aleatorio temporario gerado e criptografado
        //E depois descriptografado para verificar se é valido


        //Agora a questão é, faz sentido.. é relamente preciso ..
        //Eu mando pra voce uma chave pra voce descriptografar provando ser voce
        private record SendFileHandShake(string filename, byte[] filehash, byte[] filekey, string transferGuidId, bool acceptedRequest);
        private record FileData(string transferGuidId, byte[] Data);
        private async void SendFile()
        {
            #region Obtem o arquivo
            var path = "";
            var filedialog = new OpenFileDialog();
            var ok = filedialog.ShowDialog() ?? false;
            if (!ok)
                return;
            path = filedialog.FileName;
            #endregion
            #region Criar o hash de verificação do arquivo e faz o handshake pedindo permissão para o envio
            var FileHashKey = GenerateKey();
            var HashFileBytes = CriarHashDeArquivo(FileHashKey, path);
            var transferGuidId = Guid.NewGuid().ToString();
            //
            var FileHandShake = new SendFileHandShake(Path.GetFileName(path), HashFileBytes, FileHashKey, transferGuidId, false);
            //
            var rsaSelecteduser = RsaService.FromPublicKey(SelectedContact.PublicKey);
            var responseTask = SalkyWebSocket.WaitForAsync("route/filehs/end", TimeSpan.FromSeconds(8));
            await this.SalkyWebSocket.SendMessageServer(new WebSocket.Shared.Models.MessageServer()
            {
                Json = rsaSelecteduser.Encrypt(JsonSerializer.Serialize(FileHandShake)),
                PathString = "route/filehs/init",
                Receiver = SelectedContact.ExibitionName,
                Sender = LoggedUser.ExibitionName,
            });
            var response = await responseTask;
            ////
            var rsaCurrentUser = RsaService.FromPrivateKey(LoggedUser.PrivateKey);
            var HandShakeReponse = JsonSerializer.Deserialize<SendFileHandShake>(rsaCurrentUser.Decrypt(response.Json));
            #endregion
            #region Caso tenha dado certo, começa e a tranferencia
            if (HandShakeReponse == null || !HandShakeReponse.acceptedRequest)
            {
                new MessageBoxC("O usuario recusou o arquivo").ShowDialog();
            }
            else
            {
                //Verifica se estamos falando da mesma tranferencia.
                if (HandShakeReponse.transferGuidId == transferGuidId)
                {
                    //Começa a trasferencia
                    //Por enquanto vai ser numa porrada só..
                    var bytes = File.ReadAllBytes(path);
                    var message = new MessageServer()
                    {
                        Json = JsonSerializer.Serialize(new FileData(transferGuidId, bytes)),
                        PathString = "route/file",
                        Receiver = SelectedContact.ExibitionName,
                        Sender = LoggedUser.ExibitionName,
                    };
                    await SalkyWebSocket.SendMessageServer(message);
                }
            }
            #endregion
        }
        public static byte[] GenerateKey(int size = 512)
        {
            byte[] secretkey = new Byte[size];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(secretkey);
            return secretkey;
        }
        public static byte[] CriarHashDeArquivo(byte[] Key, string sourceFile)
        {
            byte[]? hashValue = null;
            using (HMACSHA512 hmac = new HMACSHA512(Key))
            using (FileStream inStream = new FileStream(sourceFile, FileMode.Open))
                hashValue = hmac.ComputeHash(inStream);
            return hashValue;
        }
        public static bool VerificarHashDeArquivo(byte[] Key, byte[] PreveiusHash, string sourceFile)
        {
            var newHash = CriarHashDeArquivo(Key, sourceFile);
            bool isvalid = true;

            for (int i = 0; i < PreveiusHash.Length; i++)
            {
                if (newHash[i] != PreveiusHash[i])
                {
                    isvalid = false;
                    break;
                }
            }
            return isvalid;
        }


        private string KeyToString(byte[] Key) => Convert.ToBase64String(Key);
        private byte[] KeyToByte(string Key) => Convert.FromBase64String(Key);

        private static byte[] GenerateKey()
        {
            byte[] secretkey = new Byte[64];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(secretkey);
            return secretkey;
        }




        private void RecoveryUserFileThenSet()
        {
            this.LoggedUser = this.userFileManager.GetLastLogged();
        }

        private async void BuscarUsuario()
        {
            var result = await this.SalkyWebSocket.GetUser(SearchUserText);
            if (result == null)
            {
                new MessageBoxC("Nenhum usuario com este nome encontrado").ShowDialog();
            }
            else
            {
                var contato = new ContatoVM(result);
                if (TentaAdicionarContato(contato))
                    new MessageBoxC($"{result.Apelido} foi adicionado").ShowDialog();
                else
                    new MessageBoxC($"Você já possui o {contato.ExibitionName} como amigo.").ShowDialog();

                MessageBox.Show(, "Aviso");
            }
        }
        private bool TentaAdicionarContato(ContatoVM? contato)
        {
            if (contato == null) return false;
            if (!this.LoggedUser.Contatos.Any(q => q.PublicKey.Equals(contato.PublicKey)))
            {
                this.LoggedUser.Contatos.Add(contato);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async void StartNewConfigView()
        {
            new ConfigWindow().ShowDialog();
            this.LoggedUser = this.userFileManager.ObterUsuarios().First();
            await this.SalkyWebSocket.SendThenSetUser();
        }
        public async void ChangeVisibility()
        {
            this.LoggedUser.EstaVisivel = !this.LoggedUser.EstaVisivel;
            await this.SalkyWebSocket.SendThenSetUser();
        }

        public async void SendMessage()
        {
            var msg = new MessageVM
            {
                Content = MessageToSend,
                IsOwnerMessage = true,
                IsSequencialMessage = !((SelectedContact.Messages.Count == 0) || SelectedContact.Messages.Last().IsOwnerMessage),
                SenderName = LoggedUser.ExibitionName,
                SenderPublicKey = LoggedUser.PublicKey,
                UsernameColor = LoggedUser.NickColor,
                ImageSource = LoggedUser.PictureSource,
                Date = DateTime.Now,
            };
            SelectedContact.Messages.Add(msg);
            await SalkyWebSocket.SendMessageServer(new WebSocket.Shared.Models.MessageServer()
            {
                PathString = "route/message",
                Receiver = SelectedContact.ExibitionName,
                Sender = LoggedUser.ExibitionName,
                Json = JsonSerializer.Serialize(msg),
            });
            MessageToSend = "";
        }

        public void ReceiveMessage(MessageVM message)
        {
            var contato = this.LoggedUser.Contatos.FirstOrDefault(c => c.PublicKey.Equals(message.SenderPublicKey));
            if (contato == null)
            {
                contato = new ContatoVM(message);
                this.LoggedUser.Contatos.Add(contato);
            }
            contato.Messages.Add(message);
        }




    }
}
