using Salky.App.Dtos;
using Salky.App.Services.Friends;
using Salky.App.Services.User;
using Salky.Domain.Models.FriendModels;
using Salky.WebSockets.Router.Routing.Atributes;

namespace Salky.API.WebSocketRoutes
{
    [WebSocketRoute]
    public class FriendRoute : WebSocketRouteBase
    {
        private readonly FriendService friendService;
        public FriendRoute(FriendService friendService)
        {
            this.friendService = friendService;
        }

        [WsPost("add")]
        public async Task SendFriendRequest(Guid UserToAddAsFriendId)
        {
            var friend = await friendService.SendFriendRequest(Claims.GetUserId(), UserToAddAsFriendId);
            if (friend != null)
            {
                //Envia pro usuario que adicionou
                await SendBack(friend, "friend", Method.PUT);
                //Envia pro usuario que foi adicionado
                var otherFriend = await friendService.GetById(UserToAddAsFriendId, friend.Id) ?? throw new InvalidOperationException();
                await SendToOneInPool("root", UserToAddAsFriendId, "friend", Method.PUT, otherFriend);
            }
        }

        public record UpdateFriendRequestModel(Guid FriendId, RelationShipStatus Status);
        [WsPut("status")]
        public async Task UpdateFriendRequest(UpdateFriendRequestModel updateFriendRequest)
        {
            updateFriendRequest.Deconstruct(out var FriendId, out var status);
            try
            {
                var friend = await friendService.TryUpdateFriendRequest(Claims.GetUserId(), FriendId, status);
                if (friend == null)
                {
                    await SendErrorBack("friend", $"Não foi possivel atualizar amigos.");
                    return;
                }
                if (status == RelationShipStatus.Removed || status == RelationShipStatus.Canceled || status == RelationShipStatus.Rejected)
                {
                    await SendToManyInPool("root", new[] { friend.UserId, User.UserId }, new MessageServer("friend", Method.DELETE, Status.Success, friend));
                    return;
                }
                await SendToManyInPool("root", new[] { friend.UserId, User.UserId }, new MessageServer("friend", Method.PUT, Status.Success, friend));
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao atualizar amigos.");
            }
        }

    }

}
