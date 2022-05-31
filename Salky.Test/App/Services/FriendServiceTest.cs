using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salky.App.Dtos;
using Salky.App.Services.Friends;
using Salky.App.Services.Group;
using Salky.App.Services.User;
using Salky.Domain;
using Salky.Domain.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Salky.Test.App.Services
{

    [TestClass]
    public class FriendServiceTest
    {
        private UserService userService;
        private GroupService groupService;
        private FriendService friendService;
        private FriendMessageService friendmsgService;
        private AccountService accountService;

        public FriendServiceTest()
        {
            groupService = GetRequiredService<GroupService>();
            userService = GetRequiredService<UserService>();
            friendService = GetRequiredService<FriendService>();
            friendmsgService = GetRequiredService<FriendMessageService>();
            accountService = GetRequiredService<AccountService>();
        }

       [TestMethod]
       public async Task TestFull()
        {
            Statics.CreateRandomUsers(this.accountService, 3, out var users);
            var u1 = users[0];
            var u2 = users[1];

            Assert.IsTrue(await friendService.SendFriendRequest(u1.Id, u2.Id) != null, "Não foi possivel enviar a requisição de amizade.");
            var friends = await friendService.GetAll(u1.Id);
            Assert.AreEqual(1, friends.Count, "Quantidade de amigos incompativel.");
            var friendsAccepted = await friendService.GetAllAccepted(u1.Id);
            Assert.AreEqual(0, friendsAccepted.Count, "Quantidade de amigos aprovados incompativel.");
            var friend = friends[0];
            Assert.IsTrue(await friendService.AcceptFriend(u2.Id,friend.Id) != null , "Não foi possivel aceitar a solicitação de amizade");
            friendsAccepted = await friendService.GetAllAccepted(u1.Id);
            Assert.AreEqual(1, friendsAccepted.Count, "Quantidade de amigos aprovados incompativel.");
            friendsAccepted = await friendService.GetAllAccepted(u2.Id);
            Assert.AreEqual(1, friendsAccepted.Count, "Quantidade de amigos aprovados incompativel.");
            friend = await this.friendService.GetById(u1.Id, friend.Id);
            var res = await friendmsgService.Add(u1.Id, friend.Id, "Olá");
            Assert.IsNotNull(res);
            var n = await friendmsgService.GetAll(u1.Id, friend.Id, 1, 10);
            Assert.AreEqual(1, n.Results.Count);
            //teste

            Assert.IsNull(await friendmsgService.GetAll(users[2].Id, friend.Id, 1, 10));
            var nsad = await friendmsgService.Add(users[2].Id,friend.Id,"Oq ????");
            Assert.IsNull(nsad);

        }




    }
}
