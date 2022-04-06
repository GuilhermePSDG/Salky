using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Core
{
    internal class FileSend
    {
        ///Depois do HandShake, caso o objeto volte assinado é porque o destinatario aceitou a transferencia
        ////A assinatura será um texto aleatorio temporario gerado e criptografado
        ////E depois descriptografado para verificar se é valido


        ////Agora a questão é, faz sentido.. é relamente preciso ..
        ////Eu mando pra voce uma chave pra voce descriptografar provando ser voce
        //private record SendFileHandShake(string filename, byte[] filehash, byte[] filekey, string transferGuidId, bool acceptedRequest);
        //private record FileData(string transferGuidId, byte[] Data);
        //private async void SendFile()
        //{
        //    #region Obtem o arquivo
        //    var path = "";
        //    var filedialog = new OpenFileDialog();
        //    var ok = filedialog.ShowDialog() ?? false;
        //    if (!ok)
        //        return;
        //    path = filedialog.FileName;
        //    #endregion
        //    #region Criar o hash de verificação do arquivo e faz o handshake pedindo permissão para o envio
        //    var FileHashKey = GenerateKey();
        //    var HashFileBytes = CriarHashDeArquivo(FileHashKey, path);
        //    var transferGuidId = Guid.NewGuid().ToString();
        //    //
        //    var FileHandShake = new SendFileHandShake(Path.GetFileName(path), HashFileBytes, FileHashKey, transferGuidId, false);
        //    //
        //    var rsaSelecteduser = RsaService.FromPublicKey(SelectedContact.PublicKey);
        //    var responseTask = SalkyWebSocket.WaitForAsync("route/filehs/end", TimeSpan.FromSeconds(8));
        //    await this.SalkyWebSocket.SendMessageServer(new WebSocket.Shared.Models.MessageServer()
        //    {
        //        Json = rsaSelecteduser.Encrypt(JsonSerializer.Serialize(FileHandShake)),
        //        PathString = "route/filehs/init",
        //        Receiver = SelectedContact.ExibitionName,
        //        Sender = LoggedUser.ExibitionName,
        //    });
        //    var response = await responseTask;
        //    ////
        //    var rsaCurrentUser = RsaService.FromPrivateKey(LoggedUser.PrivateKey);
        //    var HandShakeReponse = JsonSerializer.Deserialize<SendFileHandShake>(rsaCurrentUser.Decrypt(response.Json));
        //    #endregion
        //    #region Caso tenha dado certo, começa e a tranferencia
        //    if (HandShakeReponse == null || !HandShakeReponse.acceptedRequest)
        //    {
        //        new MessageBoxC("O usuario recusou o arquivo").ShowDialog();
        //    }
        //    else
        //    {
        //        //Verifica se estamos falando da mesma tranferencia.
        //        if (HandShakeReponse.transferGuidId == transferGuidId)
        //        {
        //            //Começa a trasferencia
        //            //Por enquanto vai ser numa porrada só..
        //            var bytes = File.ReadAllBytes(path);
        //            var message = new MessageServer()
        //            {
        //                Json = JsonSerializer.Serialize(new FileData(transferGuidId, bytes)),
        //                PathString = "route/file",
        //                Receiver = SelectedContact.ExibitionName,
        //                Sender = LoggedUser.ExibitionName,
        //            };
        //            await SalkyWebSocket.SendMessageServer(message);
        //        }
        //    }
        //    #endregion
        //}
        //public static byte[] GenerateKey(int size = 512)
        //{
        //    byte[] secretkey = new Byte[size];
        //    using (var rng = RandomNumberGenerator.Create())
        //        rng.GetBytes(secretkey);
        //    return secretkey;
        //}
        //public static byte[] CriarHashDeArquivo(byte[] Key, string sourceFile)
        //{
        //    byte[]? hashValue = null;
        //    using (HMACSHA512 hmac = new HMACSHA512(Key))
        //    using (FileStream inStream = new FileStream(sourceFile, FileMode.Open))
        //        hashValue = hmac.ComputeHash(inStream);
        //    return hashValue;
        //}
        //public static bool VerificarHashDeArquivo(byte[] Key, byte[] PreveiusHash, string sourceFile)
        //{
        //    var newHash = CriarHashDeArquivo(Key, sourceFile);
        //    bool isvalid = true;

        //    for (int i = 0; i < PreveiusHash.Length; i++)
        //    {
        //        if (newHash[i] != PreveiusHash[i])
        //        {
        //            isvalid = false;
        //            break;
        //        }
        //    }
        //    return isvalid;
        //}


        //private string KeyToString(byte[] Key) => Convert.ToBase64String(Key);
        //private byte[] KeyToByte(string Key) => Convert.FromBase64String(Key);

        //private static byte[] GenerateKey()
        //{
        //    byte[] secretkey = new Byte[64];
        //    using (var rng = RandomNumberGenerator.Create())
        //        rng.GetBytes(secretkey);
        //    return secretkey;
        //}

    }
}
