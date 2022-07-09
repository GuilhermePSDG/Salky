using Salky.API.Models;
using Salky.App.Dtos.Group;
using Salky.App.Services.Group;
using Salky.App.Services.User;

namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute]
    public class GroupRoute : WebSocketRouteBase
    {
        private readonly GroupService groupService;
        private readonly UserService userService;
        private readonly ILogger<GroupService> log;
        private readonly GroupMemberService groupMemberService;

        public GroupRoute(GroupService groupService, UserService userService, ILogger<GroupService> log, GroupMemberService groupMemberService)
        {
            this.groupService = groupService;
            this.userService = userService;
            this.log = log;
            this.groupMemberService = groupMemberService;
        }

        public override async Task OnConnectAsync()
        {
            await CreateUserWsIfNotCreated(new AudioState(true, true));
            var usrId = Claims.GetUserId().ToString();
            (await groupMemberService.GetAllMembersOfUser(Claims.GetUserId())).ForEach(member =>
            {
                AddOneInPool(member.GroupId.ToString(), usrId);
            });
            await base.OnConnectAsync();
        }
        public override async Task OnDisconnectAsync()
        {
            var usrId = Claims.GetUserId().ToString();
            (await groupMemberService.GetAllMembersOfUser(Claims.GetUserId())).ForEach(member =>
            {
                RemoveOneFromPool(member.GroupId.ToString(), usrId);
                RemoveOneFromPool(member.GroupId.ToString() + "/call", usrId);
            });
            await base.OnDisconnectAsync();
        }

        private async Task CreateUserWsIfNotCreated(AudioState audioState)
        {
            var usr = await userService.GetUserById(Claims.GetUserId());
            if (usr == null) throw new NullReferenceException("Usuario não encontrado");
            var userCall = new GroupMemberCall(audioState);
            Storage.AddOrUpdate(userCall);
        }


        public record ChangeName(Guid GroupId, string NewGroupName);
        [WsPost("change/name")]
        public async Task ChangeGroupName(ChangeName changeName)
        {
            try
            {
                log.LogInformation("Mudando nome do grupo");
                changeName.Deconstruct(out var GroupId, out var NewGroupName);
                var groupDto = await groupService.ChangeGroupName(Claims.GetUserId(), GroupId, NewGroupName);
                if (groupDto == null)
                {
                    await SendErrorBack(CurrentPath, "Não foi possível mudar o nome do grupo.");
                    return;
                }
                else
                {
                    await SendToAllInPool(GroupId.ToString(), CurrentPath, Method.PUT, new ChangeName(groupDto.Id, groupDto.Name));
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Erro ao mudar o nome do grupoo");
                await SendErrorBack(CurrentPath, "Erro ao mudar o nome do grupoo");
            }
        }

        [WsPost("create")]
        public async Task Create(string GroupName)
        {
            var uId = Claims.GetUserId();
            var group = await groupService.CreateNewPublicGroup(uId, GroupName);
            var member = await groupMemberService.GetMemberWithRole(uId, group.Id) ?? throw new InvalidOperationException();
            await AddOneInPool(group.Id.ToString(), uId.ToString());
            await SendBack(group, CurrentPath, Method.POST);
        }

        [WsDelete]
        public async Task DeleteGroup(Guid GroupId)
        {
            try
            {
                var removed = await groupService.RemoveGroup(Claims.GetUserId(), GroupId);
                if (removed)
                {
                    await SendToAllInPool(GroupId.ToString(), CurrentPath, Method.DELETE, GroupId);
                    await DeletePool(GroupId.ToString());
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possível remover o grupo");
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao remover o grupo");
            }
        }


    }
}
