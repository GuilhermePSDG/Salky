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

    public class ContactServiceTest
    {
        private UserService userService;
        private ContactService contactService;
        private UserLoggedDto usr1;
        private UserLoggedDto usr2;

        public ContactServiceTest()
        {
            contactService = GetService<ContactService>();
            userService = GetService<UserService>();
            CreateUsers(out usr1, out usr2);
        }

        [TestMethod("Tenta adicionar contato pelo nome")]
        public void TesteAdicionarContato()
        {
            var contact = contactService.AddContactByUserNameAsync(usr1.Id, usr2.UserName).Result;
            Assert.IsNotNull(contact, "Não foi possivel adicionar o contato");
            Assert.IsTrue(contact.UserName == usr2.UserName, "UserName do contato retornado é diferente do contado adicionado");

            var contact2 = contactService.AddContactByUserNameAsync(usr2.Id, usr1.UserName).Result;
            Assert.IsNotNull(contact2, "O 'Usuario 2' não conseguiu adicionar o 'Usuario 1' nos contatos");
            Assert.IsTrue(contact2.UserName == usr1.UserName, "UserName do contato2 retornado é diferente do contado adicionado");
        }

        [TestMethod("Tenta se adicionar como contato")]
        public void TesteSeAdicionarComoContato()
        {
            try
            {
                var contact = contactService.AddContactByUserNameAsync(usr1.Id, usr1.UserName).Result;
                Assert.IsNull(contact, "Foi possivel se adicionar como contato");
            }
            catch { }
        }

        [TestMethod("Tenta obter contato pelo Id")]
        public void TestaObterContato()
        {
            CreateUsers(out var inUsr1, out var inUsr2);
            var contact = contactService.AddContactByUserNameAsync(inUsr1.Id, inUsr2.UserName).Result;
            var contactsRecovered = contactService.GetAllAsync(inUsr1.Id).Result;
            Assert.IsTrue(contactsRecovered.Count == 1, "Quantidade contatos invalida, falha na recuperação dos contatos");
            Assert.IsTrue(contact.UserName == contactsRecovered.First().UserName, "Usuario recupera diferente do usuario adicionado");
        }

        [TestMethod("Tenta obter um contato pelo nome")]
        public void TestaObterContatoPeloNome()
        {
            CreateUsers(out var inUsr1, out var inUsr2);
            var contato = contactService.GetUserByName(inUsr1.UserName).Result;
            Assert.IsNotNull(contato, "Não foi possivel encontrar um contato existente pelo nome");
        }

        private void CreateUsers(out UserLoggedDto user1, out UserLoggedDto user2)
        {
            userService.CreateRandomUsers(2, out var usrs);
            user1 = usrs[0];
            user2 = usrs[1];
        }



    }


}
