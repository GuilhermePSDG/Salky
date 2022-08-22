using Salky.App.Services.Friends;
using Salky.App.Services.Group;
using Salky.App.Services.User;

namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute("group/member")]
    public class GroupMemberRoute : WebSocketRouteBase
    {
        private readonly GroupService groupService;
        public UserService userService;
        private readonly ILogger<GroupService> log;
        private readonly GroupMemberService groupMemberService;

        public GroupMemberRoute(
            GroupService groupService,
            UserService userService,
            ILogger<GroupService> log,
            GroupMemberService groupMemberService
            )
        {
            this.groupService = groupService;
            this.userService = userService;
            this.log = log;
            this.groupMemberService = groupMemberService;
        }

        public record AddFriendInGroupModel(Guid FriendId, Guid GroupId);
        [WsPost]
        public async Task AddFriendInGroup(AddFriendInGroupModel addUserInGroup)
        {
            try
            {
                addUserInGroup.Deconstruct(out var FriendId, out var groupId);
                var newMember = await groupMemberService.AddNewMemberByFriendId(Claims.GetUserId(), FriendId, groupId);
                if (newMember != null)
                {
                    await SendToAllInPool(groupId, CurrentPath, Method.POST, newMember);
                    var groupDto = await groupService.GetById(newMember.UserId, newMember.GroupId);
                    await SendToOneInPool("root", newMember.UserId, "group/create", Method.POST, groupDto!);
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
        public record MemberRemoved(Guid MemberId, Guid GroupId);

        [WsDelete]
        public async Task DeleteMember(Guid MemberId)
        {
            try

            {
                var removedMember = await groupMemberService.RemoveGroupMember(Claims.GetUserId(), MemberId);
                if (removedMember != null)
                {
                    var (groupId, removedMemberUsrId) = ((Key)removedMember.GroupId, (Key)removedMember.UserId);
                    var data = new MemberRemoved(removedMember.Id, removedMember.GroupId);
                    await RemoveOneFromPool(groupId, removedMemberUsrId);
                    await Task.WhenAll(
                        SendToOneInPool("root", removedMemberUsrId, "group", Method.DELETE, groupId.Value),
                        SendToAllInPool(
                              PoolKey: groupId,
                              Path: CurrentPath, Method.DELETE, data
                              )
                        );
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
