using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salky.App.Dtos;
using Salky.App.Services.Group;
using Salky.App.Services.User;
using Salky.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Salky.Test.App.Services
{
    [TestClass]
    public class MessageServiceTest
    {

        private GroupMessageService messageService;
        private GroupMemberService groupMemberService;
        private AccountService accountService;
        private GroupService groupService;
        private UserService userService;

        public MessageServiceTest()
        {
            groupService = GetRequiredService<GroupService>();
            userService = GetRequiredService<UserService>();
            messageService = GetRequiredService<GroupMessageService>();
            groupMemberService = GetRequiredService<GroupMemberService>();
            accountService = GetRequiredService<AccountService>();
        }

        [TestMethod]
        public async Task TesteObterMensagems()
        {
            accountService.CreateRandomUsers(3, out var users);
            var groupName = GetRandomSimpleString;
            var group = await groupService.CreateNewPublicGroup(users[0].Id, groupName);
            await groupMemberService.AddNewMemberByFriendId(users[0].Id,users[1].Id, group.Id);
            await groupMemberService.AddNewMemberByFriendId(users[0].Id,users[2].Id, group.Id);

            var msg1 = "First Message"+ users[0].UserName;
            var msg2 = "Second Message"+ users[1].UserName;
            var msg3 = "Third Message" + users[2].UserName;

            await this.messageService.AddMessage(users[0].Id, group.Id,msg1 );
            await this.messageService.AddMessage(users[0].Id, group.Id,msg2 );
            await this.messageService.AddMessage(users[0].Id, group.Id,msg3 );

            var msgsOfGroup = (await this.messageService.GetMessagesOfGroup(users[0].Id, group.Id, 1, 20)).Results;
            Assert.IsTrue(msgsOfGroup.Count == 3, "Total de mensagens no grupo incompativel");
            Assert.IsTrue(msgsOfGroup[0].Content == msg1, "Mensagem Invalida");
            Assert.IsTrue(msgsOfGroup[1].Content == msg2, "Mensagem Invalida");
            Assert.IsTrue(msgsOfGroup[2].Content == msg3, "Mensagem Invalida");

        }


    }
}
