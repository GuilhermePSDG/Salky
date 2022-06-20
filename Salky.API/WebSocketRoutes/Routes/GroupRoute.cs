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
                AddInPool(member.GroupId.ToString(),usrId);
            });
        }

    
        [WsAfterConnectionClosed]
        public async Task AfterClose()
        {
            var usrId = Claims.GetUserId().ToString();
            (await groupMemberService.GetAllMembersOfUser(Claims.GetUserId())).ForEach(member =>
            {
                TryRemoveFromPool(member.GroupId.ToString(), usrId);
                TryRemoveFromPool(member.GroupId.ToString()+"/call", usrId);
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
            var member = await this.groupMemberService.GetMemberWithRole(uId, group.Id) ?? throw new InvalidOperationException();
            //PROBLEMA DE ID
            AddInPool(group.Id.ToString(), uId.ToString());
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
                    DeletePool(GroupId.ToString());
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
