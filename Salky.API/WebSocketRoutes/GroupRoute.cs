using Salky.API.Models;
using Salky.App.Dtos.Group;
using Salky.App.Services.Group;
using Salky.App.Services.User;



namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute("group")]
    public class GroupRoute : WebSocketRouteBaseCustom
    {
        private readonly GroupService groupService;
        private readonly IConnectionMannager mannager;
        private readonly UserService userService;
        private readonly ILogger<GroupService> log;
        private readonly GroupMemberService groupMemberService;

        public GroupRoute(GroupService groupService, IConnectionMannager mannager, UserService userService, ILogger<GroupService> log, GroupMemberService groupMemberService)
        {
            this.groupService = groupService;
            this.mannager = mannager;
            this.userService = userService;
            this.log = log;
            this.groupMemberService = groupMemberService;
        }

        public override async Task OnDisconnectAsync()
        {
            if (UserStorage.Get("GROUP_LISTEN_ID") is Guid groupId)
            {
                await RemoveOneFromPool(groupId, User.UserId);
                UserStorage.Remove("GROUP_LISTEN_ID");
            }
            await base.OnDisconnectAsync();
        }

        [WsListener("entry")]
        public async Task ListenerGroup(Guid GroupId)
        {
            if (!await this.groupMemberService.UserMakePartOfGroup(Guid.Parse(User.UserId), GroupId))
            {
                await SendErrorBack(CurrentRoutePath.Path, "User do not make part of group", CurrentRouteMethod);
                return;
            }
            if (IsInPool(GroupId, User.UserId))
            {
                await SendErrorBack(CurrentRoutePath.Path, "User is alredy in pool", CurrentRouteMethod);
                return;
            }
            if (!await AddOneInPool(GroupId, User.UserId))
            {
                await SendErrorBack(CurrentRoutePath.Path, "Unable to add listener", CurrentRouteMethod);
            }
            UserStorage.AddOrUpdate("GROUP_LISTEN_ID", GroupId);
            //await SendBack("", CurrentPath, CurrentRouteMethod, Status.Success);
        }
        [WsListener("leave")]
        public async Task RemoveListener(Guid GroupId)
        {
            if (await RemoveOneFromPool(GroupId, User.UserId) == 1)
            {
                UserStorage.Remove("GROUP_LISTEN_ID");
                //await SendBack("", CurrentPath, CurrentRouteMethod, Status.Success);
            }
            else
            {
                await SendErrorBack(CurrentRoutePath.Path, "Unable to remove listener", CurrentRouteMethod);
            }
        }


        public record ChangeName(Guid GroupId, string NewGroupName);
        [WsPost("change/name")]
        public async Task ChangeGroupName(ChangeName changeName)
        {
            try
            {
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
                    await DeletePool(GroupId);
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
