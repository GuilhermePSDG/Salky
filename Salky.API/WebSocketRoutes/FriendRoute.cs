using Salky.App.Dtos;
using Salky.App.Services.Friends;
using Salky.App.Services.User;
using Salky.Domain.Models.FriendModels;

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

        public override async Task OnConnectAsync()
        {
            var usrId = Claims.GetUserId();
            (await friendService.GetAll(Claims.GetUserId()))
                .ForEach(friend => AddOneInPool(friend.Id, usrId)
            );
            await base.OnConnectAsync();
        }
        public override async Task OnDisconnectAsync()
        {
            var usrId = Claims.GetUserId();
            (await friendService.GetAll(Claims.GetUserId()))
                .ForEach(friend => RemoveOneFromPool(friend.Id, usrId));
            await base.OnDisconnectAsync();
        }

        [WsPost("add")]
        public async Task SendFriendRequest(Guid UserToAddAsFriend)
        {
            var friend = await friendService.SendFriendRequest(Claims.GetUserId(), UserToAddAsFriend);
            if (friend != null)
            {
                AddManyInPool(friend.Id, Claims.GetUserId(), UserToAddAsFriend);
                //Envia pro usuario que adicionou
                await SendBack(friend, "friend", Method.PUT);
                //Envia pro usuario que foi adicionado
                var otherFriend = await friendService.GetById(UserToAddAsFriend, friend.Id) ?? throw new InvalidOperationException();
                await SendToOneInPool(friend.Id, UserToAddAsFriend, "friend", Method.PUT, otherFriend);
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
                    await SendToAllInPool(FriendId, "friend", Method.DELETE, friend);
                    DeletePool(FriendId);
                    return;
                }
                await SendToAllInPool(FriendId, "friend", Method.PUT, friend);
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao atualizar amigos.");
            }
        }

    }

}
