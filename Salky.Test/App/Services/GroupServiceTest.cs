using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Salky.App.Dtos;
using Salky.App.Interfaces;
using Salky.App.Services.Group;
using Salky.App.Services.User;
using Salky.Domain;
using Salky.Domain.Models.GroupModels;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Salky.Test.App.Services
{
    [TestClass]
    public class GroupServiceTest
    {
        private UserService userService;
        private GroupService groupService;
        private GroupMemberService groupMemberService;
        private IAccountService accountService;

        public GroupServiceTest()
        {
            groupService = GetRequiredService<GroupService>();
            userService = GetRequiredService<UserService>();
            groupService = GetRequiredService<GroupService>();
            groupMemberService = GetRequiredService<GroupMemberService>();
            accountService = GetRequiredService<IAccountService>();
        }

        [TestMethod("Testa adicionar usuario em um grupo publico")]
        public async Task TestAddUserInGroup()
        {
            Statics.CreateRandomUsers(GetRequiredService<IAccountService>(), 3, out var users);
            var gName = GetRandomSimpleString;

            var group = await this.groupService.CreateNewPublicGroup(users[0].Id, gName);
            var g1 = await this.groupMemberService.AddNewMemberByFriendId(users[0].Id,users[1].Id,group.Id);
            Assert.IsNotNull(g1);
            var g2 = await this.groupMemberService.AddNewMemberByFriendId(users[0].Id,users[2].Id, group.Id);
            Assert.IsNotNull(g2);
            var members = await this.groupMemberService.GetMembersOfGroup(users[0].Id, group.Id);
            Assert.IsNotNull(members);
            Assert.AreEqual(3, members.Count);


            var member = await this.groupMemberService.GetMemberWithRole(users[0].Id, group.Id);
            var member2 = await this.groupMemberService.GetMemberWithRole(users[1].Id, group.Id);
            var member3 = await this.groupMemberService.GetMemberWithRole(users[2].Id, group.Id);
            return;

        }
        [TestMethod]
        public async Task TestCreateNewPublicGroup()
        {
            Statics.CreateRandomUsers(accountService, 3, out var users);
            var gName = GetRandomSimpleString;
            var group = await this.groupService.CreateNewPublicGroup(users[0].Id, gName);
        }


    }


}
