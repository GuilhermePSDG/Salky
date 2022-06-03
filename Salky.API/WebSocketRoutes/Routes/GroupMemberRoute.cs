using Salky.App.Services.Group;
using Salky.App.Services.User;

namespace Salky.API.WebSocketRoutes.Routes
{
    [WebSocketRoute("group/member")]
    public class GroupMemberRoute : WebSocketRouteBase
    {
        private readonly GroupService groupService;
        public UserService userService;
        private readonly ILogger<GroupService> log;
        private readonly GroupMemberService groupMemberService;

        public GroupMemberRoute(GroupService groupService, UserService userService, ILogger<GroupService> log, GroupMemberService groupMemberService)
        {
            this.groupService = groupService;
            this.userService = userService;
            this.log = log;
            this.groupMemberService = groupMemberService;
        }


        public record AddFriendInGroupModel(Guid FriendId, Guid GroupId);
        [WsPost()]
        public async Task AddFriendInGroup(AddFriendInGroupModel addUserInGroup)
        {
            try
            {
                addUserInGroup.Deconstruct(out var FriendId, out var groupId);
                var newMember = await groupMemberService.AddNewMemberByFriendId(Claims.GetUserId(), FriendId, groupId);
                if (newMember != null)
                {
                    var pool = GetPool(groupId.ToString());
                    await pool.SendToAll(CurrentPath, Method.POST, newMember);

                    if(RootConnectionMannager.TryGetSocket(newMember.UserId.ToString(),out var usrSocket))
                    {
                        this.AddInPool(groupId.ToString(),newMember.Id.ToString() , usrSocket);
                        var groupDto = await this.groupService.GetById(newMember.UserId, newMember.GroupId) ?? throw new InvalidOperationException();
                        await usrSocket.SendMessageServer(new("group/create",Method.POST, Status.Success,groupDto));
                    }
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possível adicionar o usuario no grupo");
                }

            }
            catch (Exception ex)
            {
                log.LogError(ex, "Erro ao adicionar o usuario no grupo");
                await SendErrorBack(CurrentPath, "Erro ao adicionar o usuario no grupo");
            }
        }

        public record RemoveMember(Guid MemberId, Guid GroupId);
        [WsDelete]
        public async Task DeleteMember(Guid MemberId)
        {
            try
            {
                var removedMember = await groupMemberService.RemoveGroupMember(Claims.GetUserId(), MemberId);
                if (removedMember != null)
                {
                    var data = new RemoveMember(removedMember.Id, removedMember.GroupId);

                    var pool = GetPool(removedMember.GroupId.ToString());
                    if(pool.TryRemoveSocket(removedMember.Id.ToString(), out var removedsock))
                    {
                        await removedsock.SendMessageServer(new MessageServer("group", Method.DELETE, Status.Success));
                    }
                    await GetPool(removedMember.GroupId.ToString()).SendToAll(CurrentPath, Method.DELETE, data);
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possível remover o membro no grupo");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Erro ao remover o membro do grupo");
                await SendErrorBack(CurrentPath, "Erro ao remover o membro do grupo");
            }
        }



    }
}
