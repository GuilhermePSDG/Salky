//using System;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.Json;
//using System.Text.RegularExpressions;
//using WebSocket.Shared;
//using WebSocket.Shared.DataAcess.Local;
//using WebSocket.Shared.DataAcess.Models;



////var Giga1 = 1000000000;
////var MB = 1048576;


////var rng = RandomNumberGenerator.Create();
////var password = new byte[1024];
////rng.GetBytes(password);
////var text = new byte[MB];
////rng.GetBytes(text);

////var aeService = new AESservice();
////var encryptedData = aeService.Encrypt(password, text);
////var decryptedData = aeService.Decrypt(password,encryptedData);

////var isEqual = decryptedData.Equals(text);

////var encryptedData2 = aeService.Encrypt(password, text);
////var decryptedData2 = aeService.Decrypt(password, encryptedData);

////var isEqual2 = decryptedData2.Equals(text);

////var isEqual3 = encryptedData2.SequenceEqual(encryptedData);


////return;

////string password = "ThePasswordToDecryptAndEncryptTheFile";
////// For additional security Pin the password of your files
////GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);
////// Encrypt the file
////AESservice.FileEncrypt(@"C:\Users\username\Desktop\wordFileExample.doc", password);
////// To increase the security of the encryption, delete the given password from the memory !
////AESservice.ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
////gch.Free();
////// You can verify it by displaying its value later on the console (the password won't appear)
////Console.WriteLine("The given password is surely nothing: " + password);
////return;




//var rsaService = RsaService.GenereteNewKeys(10240);
//var rsaService2 = RsaService.GenereteNewKeys(10240);
//var FenixUsuario = new Usuario()
//{
//    ExibitionName = "Fenix",
//    PictureSource = "",
//    PrivateKey = rsaService.GetPrivateKey(),
//    PublicKey = rsaService.GetPublicKey(),
//    VisivelUltimoLogin = true,
//};
//Console.WriteLine("Fenix Criado");
//var RobertoUsuario = new Usuario()
//{
//    ExibitionName = "Roberto",
//    PictureSource = "",
//    PrivateKey = rsaService2.GetPrivateKey(),
//    PublicKey = rsaService2.GetPublicKey(),
//    VisivelUltimoLogin = true,
//};

//FenixUsuario.Contatos.Add(new Contato()
//{
//    ExibitionName = RobertoUsuario.ExibitionName,
//    NickColor = RobertoUsuario.NickColor,
//    PictureSource = RobertoUsuario.PictureSource,
//    PublicKey = RobertoUsuario.PublicKey,
//});

//RobertoUsuario.Contatos.Add(new Contato()
//{
//    ExibitionName = FenixUsuario.ExibitionName,
//    NickColor = FenixUsuario.NickColor,
//    PictureSource = FenixUsuario.PictureSource,
//    PublicKey = FenixUsuario.PublicKey,
//});



//var FenixSalkyClient = await SalkyWebSocketClient.StartNewAsync(
//    () => new WebSocket.Shared.Models.UserServer(FenixUsuario.ExibitionName,true, FenixUsuario.PublicKey),
//    usrServer => FenixUsuario.ExibitionName = usrServer.Apelido,
//    FenixUsuario.PrivateKey,
//    new Uri("ws://localhost:5281")
//    );



//var RobertoSalkyClient = await SalkyWebSocketClient.StartNewAsync(
//    () => new WebSocket.Shared.Models.UserServer(RobertoUsuario.ExibitionName, true, RobertoUsuario.PublicKey),
//    usrServer => RobertoUsuario.ExibitionName = usrServer.Apelido,
//    RobertoUsuario.PrivateKey,
//    new Uri("ws://localhost:5281")
//    );


//FenixSalkyClient.OnClient<Message>((msg) =>
//{
//    Console.WriteLine($" O {msg.Sender.ExibitionName} disse para {msg.Receiver.ExibitionName} : {msg.Content}");
//});

//while (true)
//{
//    await RobertoSalkyClient.SendGet<Message>(new Message()
//    {
//        Content = "Fala fenix",
//        Date = DateTime.Now,
//        Receiver = FenixUsuario,
//        Sender = RobertoUsuario,
//    }, 
//    FenixUsuario.PublicKey);
//    Console.ReadLine();
//}
