using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salky.App.Dtos;
using Salky.App.Dtos.Auth;
using Salky.App.Services;
using Salky.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Salky.Test.App.Services
{
    [TestClass]
    public class MessageServiceTest
    {

        private MessageService messageService;
        private ContactService contactService;
        private UserService userService;

        public MessageServiceTest()
        {
            contactService = GetService<ContactService>();
            userService = GetService<UserService>();
            messageService = GetService<MessageService>();
        }


        [TestMethod]
        public async Task TestaEnvioDeMensagem()
        {
            userService.CreateRandomUsers(2, out var users);
            var contato1 = await contactService.AddContactByUserNameAsync(users[0].Id, users[1].UserName);
            var contato2 = await contactService.AddContactByUserNameAsync(users[1].Id, users[0].UserName);

            var ok = await messageService.AdicionarMensagemParaAmbos(users[0].Id, contato1.ContactId, "Olá contato 2");
            Assert.IsTrue(ok != null, "Não foi possivel enviar a mensagem");
        }
        [TestMethod]
        public async Task TesteObterMensagems()
        {
            userService.CreateRandomUsers(3, out var users);
            var contato1 = await contactService.AddContactByUserNameAsync(users[0].Id, users[1].UserName);
            var contato2 = await contactService.AddContactByUserNameAsync(users[1].Id, users[0].UserName);

            var contato3 = await contactService.AddContactByUserNameAsync(users[0].Id, users[2].UserName);
            var contato4 = await contactService.AddContactByUserNameAsync(users[2].Id, users[0].UserName);



            //Contato 1 manda para contato 2
            await messageService.AdicionarMensagemParaAmbos(users[0].Id, contato1.ContactId, "Olá contato 2");
            await messageService.AdicionarMensagemParaAmbos(users[0].Id, contato1.ContactId, "Como vai contato 2 ?");

            //Contato 2 manda para contato 1
            await messageService.AdicionarMensagemParaAmbos(users[1].Id, contato2.ContactId, "Vou bem contato 1");
            //Contato 2 manda para contato 1
            await messageService.AdicionarMensagemParaAmbos(users[1].Id, contato2.ContactId, "E você contato 1 ?");




            //Uma conversa entre o usuario 0 e 2, só pra ficar de entruso 
            await messageService.AdicionarMensagemParaAmbos(users[0].Id, contato3.ContactId, "E você contato 4 ?");
            await messageService.AdicionarMensagemParaAmbos(users[2].Id, contato4.ContactId, "E você contato 3 ?");

            var res2 = await messageService.GetMessagesByContactId(users[0].Id, contato3.ContactId);
            Assert.IsTrue(res2.Count == 2, "Quantidade de mensagems incompativeis");
            Assert.IsTrue(res2[0].UserSenderId == users[0].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");
            Assert.IsTrue(res2[1].UserSenderId == users[2].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");
            Assert.IsTrue(res2[0].UserReceiverId == users[2].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");
            Assert.IsTrue(res2[1].UserReceiverId == users[0].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");



            var res = await messageService.GetMessagesByContactId(users[0].Id, contato1.ContactId);

            Assert.IsTrue(res.Count == 4, "Quantidade de mensagems incompativeis");

            Assert.IsTrue(res[0].UserSenderId == users[0].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");
            Assert.IsTrue(res[1].UserSenderId == users[0].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");

            Assert.IsTrue(res[2].UserSenderId == users[1].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");
            Assert.IsTrue(res[3].UserSenderId == users[1].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");


            Assert.IsTrue(res[0].UserReceiverId == users[1].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");
            Assert.IsTrue(res[1].UserReceiverId == users[1].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");

            Assert.IsTrue(res[2].UserReceiverId == users[0].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");
            Assert.IsTrue(res[3].UserReceiverId == users[0].Id, "Incompatibilidade entre quem enviou e quem recebeu a mensagem");


        }


    }
}
