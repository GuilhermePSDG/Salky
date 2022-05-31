using Salky.API.WebSocketRoutes.Models;
using Salky.App.Dtos.Group;
using Salky.App.Services.Group;
using Salky.App.Services.User;
using Salky.WebSocket.Infra.Interfaces;

namespace Salky.API.WebSocketRoutes.Routes
{
    [WebSocketRoute]
    public class GroupRoute : WebSocketRouteBase
    {
        private readonly GroupService groupService;
        public UserService userService;
        private readonly ILogger<GroupService> log;
        private readonly GroupMemberService groupMemberService;

        public GroupRoute(GroupService groupService, UserService userService, ILogger<GroupService> log, GroupMemberService groupMemberService)
        {
            this.groupService = groupService;
            this.userService = userService;
            this.log = log;
            this.groupMemberService = groupMemberService;
        }


        [WsAfterConnectionEstablished]
        public async Task AfterOpen()
        {
            await CreateUserWsIfNotCreated(new AudioState(true, true));
            var usrId = Claims.GetUserId().ToString();
            (await groupMemberService.GetAllMembersOfUser(Claims.GetUserId())).ForEach(member =>
            {
                AddInPool(member.GroupId.ToString(),member.Id.ToString(), UserSocket);
            });
        }

    
        [WsAfterConnectionClosed]
        public async Task AfterClose()
        {
            var usrId = Claims.GetUserId().ToString();
            (await groupMemberService.GetAllMembersOfUser(Claims.GetUserId())).ForEach(member =>
            {
                TryRemoveFromPool(member.GroupId.ToString(), member.Id.ToString());
            });
        }
            
        private async Task CreateUserWsIfNotCreated(AudioState audioState)
        {
            if (!Storage.TryGet<GroupMemberCall>(out var usrCall))
            {
                try
                {
                    var usr = await userService.GetUserById(Claims.GetUserId());
                    if (usr == null) throw new NullReferenceException("Usuario não encontrado");
                    var userCall = new GroupMemberCall(audioState);
                    Storage.Add(userCall);
                }
                catch (Exception ex)
                {
                    log.LogError(ex, "");
                }
            }
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
                    var pool = GetPool(GroupId.ToString());
                    await pool.SendToAll(CurrentPath, Method.PUT, new ChangeName(groupDto.Id, groupDto.Name));
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
            var group = await groupService.CreateNewPublicGroup(Claims.GetUserId(), GroupName);
            var member = await this.groupMemberService.GetMemberWithRole(Claims.GetUserId(), group.Id) ?? throw new InvalidOperationException();
            AddInPool(group.Id.ToString(), member.Id.ToString(),UserSocket);
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
                    var pool = GetPool(GroupId.ToString());
                    await pool.SendToAll(CurrentPath, Method.DELETE, GroupId);
                    pool.Previus.TryRemoveConnectionPool(GroupId.ToString(),out _);
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possível remover o grupo");
                }
            }
            catch (Exception ex)
            {
                await SendErrorBack(CurrentPath, "Erro ao remover o grupo");
            }
        }


    }
}
