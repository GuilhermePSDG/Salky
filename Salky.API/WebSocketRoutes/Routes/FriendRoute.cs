using Salky.API.WebSocketRoutes.Models;
using Salky.App.Dtos;
using Salky.App.Services.Friends;
using Salky.App.Services.User;
using Salky.Domain.Models.FriendModels;

namespace Salky.API.WebSocketRoutes.Routes
{
    [WebSocketRoute]
    public class FriendRoute : WebSocketRouteBase
    {
        private readonly FriendService friendService;
        public FriendRoute(FriendService friendService,  UserService userService)
        {
            this.friendService = friendService;
        }

        [WsAfterConnectionEstablished]
        public async Task AfterOpen()
        {
            var usrId = Claims.GetUserId().ToString();
            (await friendService.GetAll(Claims.GetUserId()))
                .ForEach(friend => AddInPool(friend.Id.ToString(),usrId)
            );
        }

        [WsAfterConnectionClosed]
        public async Task AfterClose()
        {
            var usrId = Claims.GetUserId().ToString();
            (await friendService.GetAll(Claims.GetUserId()))
                .ForEach(friend => TryRemoveFromPool(friend.Id.ToString(), usrId));
        }

        [WsPost("add")]
        public async Task SendFriendRequest(Guid UserToAddAsFriend)
        {

            var friend = await this.friendService.SendFriendRequest(Claims.GetUserId(), UserToAddAsFriend);
            if (friend != null)
            {
                //Envia pro usuario que adicionou
                AddInPool(friend.Id.ToString(), Claims.GetUserId().ToString(), UserToAddAsFriend.ToString());
                await SendBack(friend, "friend", Method.PUT);
                //Envia pro usuario que foi adicionado
                var otherFriend = await this.friendService.GetById(UserToAddAsFriend, friend.Id) ?? throw new InvalidOperationException();
                await SendToOneInPool(friend.Id.ToString(), UserToAddAsFriend.ToString(), "friend", Method.PUT, otherFriend);
            }
            else
            {

            }
        }

        public record UpdateFriendRequestModel(Guid FriendId,RelationShipStatus Status);

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
                    await SendToAllInPool(FriendId.ToString(), "friend", Method.DELETE, friend);
                    DeletePool(FriendId.ToString());
                    return;
                }
                await SendToAllInPool(FriendId.ToString(), "friend", Method.PUT, friend);
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao atualizar amigos.");
            }
        }


    }

}
