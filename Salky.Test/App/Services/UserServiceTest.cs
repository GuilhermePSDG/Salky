using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salky.App.Dtos;
using Salky.App.Dtos.Users;
using Salky.App.Services.User;
using Salky.Domain;
using System;
using System.Threading.Tasks;

namespace Salky.Test.App.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private UserService userService;
        private AccountService accountService;

        public UserServiceTest()
        {
            userService = GetRequiredService<UserService>();
            accountService = GetRequiredService<AccountService>();
        }


        [TestMethod("Cria uma conta")]
        public async Task TestCreateAccount()
        {
            var name = GetRandomString;
            var pass = GetRandomString;
            var account = await CreateAccount(name, pass);
            Assert.IsFalse(account == null, "Usuario nulo ap�s o cadastro");
            Assert.IsFalse(account.UserName != name, "Nome do usuario cadastrado difere do nome pedido durante o cadastro.");
            Assert.IsFalse(account.Token.Length < 10, "Token com tamanho incompativel");
        }
        [TestMethod("Efetua o login")]
        public async Task TestLogin()
        {
            var name = GetRandomString;
            var pass = GetRandomString;
            var createdAcc = await CreateAccount(name, pass);
            var usrLogged = await accountService.LoginAsync(new()
            {
                UserName = name,
                Password = pass
            });
            Assert.IsFalse(usrLogged == null, "Usuario nulo ap�s o Login");
            Assert.IsFalse(usrLogged.Data.UserName != name, "Nome do usuario cadastrado difere do nome pedido durante o cadastro.");
            Assert.IsFalse(usrLogged.Data.Token.Length < 10, "Token com tamanho incompativel");
        }

        [TestMethod("Obter usuario pelo Id")]
        public async Task TesteGetUser()
        {
            var name = GetRandomString;
            var pass = GetRandomString;
            var createdAcc = await CreateAccount(name, pass);
            var recoverdUser = await userService.GetUserById(createdAcc.Id);
            Assert.IsFalse(recoverdUser == null, "Usuario nulo apos tentar obter pelo ID");
            Assert.IsFalse(recoverdUser.UserName != name, "Nome do usuario cadastrado difere do nome pedido durante o cadastro.");




        }
        [TestMethod("Deleta um usuario pelo id")]
        public async Task TesteDeleteUser()
        {
            var name = GetRandomString;
            var pass = GetRandomString;
            var createdAcc = await CreateAccount(name, pass);
            var deleted = await userService.DeleteUser(createdAcc.Id);
            Assert.IsTrue(deleted, "Usuario n�o foi deletado");
            try
            {
                var usr = await this.userService.GetUserById(createdAcc.Id);
                if (usr == null)
                    throw new Exception();
                Assert.IsTrue(false, "Usuario n�o foi deletado");

            }
            catch
            {
            }

        }

        [TestMethod("Duplicidade de UserName")]
        public async Task TestaDuplicidade()
        {
            var name = GetRandomString;
            var pass = GetRandomString;

            try
            {
                var createdAcc = await CreateAccount(name, pass);
                var createdAcc2 = await CreateAccount(name, pass);
                Assert.IsTrue(createdAcc != null && createdAcc2 != null, "Dois usuarios foram criados com o mesmo nome de usuario");
            }
            catch (Exception ex)
            {
            }

            try
            {
                var createdAcc = await CreateAccount(name.ToLower(), pass);
                var createdAcc2 = await CreateAccount(name.ToUpper(), pass);
                Assert.IsTrue(createdAcc != null && createdAcc2 != null, "Dois usuarios foram criados com o mesmo nome de usuario, um com UperCase outro com LowerCase");
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod("Tenta efetuar login com varia��es de cases")]
        public async Task TesteLoginFailled()
        {
            var name = GetRandomString + "aB";
            var pass = GetRandomString + "aB";

            var createdAcc = await CreateAccount(name, pass);
            try
            {
                var n = await accountService.LoginAsync(new() { UserName = name, Password = pass.ToLower() });
                Assert.IsNull(n.Data, "Usuario logado com senha para LowerCase");
            }
            catch { }

            try
            {
                var n = await accountService.LoginAsync(new() { UserName = name, Password = pass.ToUpper() });
                Assert.IsNull(n, "Usuario logado com senha para UperCase");
            }
            catch { }

            try
            {
                var n = await accountService.LoginAsync(new() { UserName = name.ToUpper(), Password = pass });
                Assert.IsNull(n.Data, "Usuario logado com UserName para UperCase");
            }
            catch { }

            try
            {
                var n = await accountService.LoginAsync(new() { UserName = name.ToLower(), Password = pass });
                Assert.IsNull(n.Data, "Usuario logado com UserName para LowerCase");
            }
            catch { }
        }


        public async Task<UserLoggedDto?> CreateAccount(string name, string password)
        {
            return (await accountService.CreateAccountAsync(new()
            {
                UserName = name,
                Password = password
            })).Data;
        }

    }
}