using Salky.API.WebSocketRoutes.Models;
using Salky.App.Services.Friends;
using Salky.App.Services.User;

namespace Salky.API.WebSocketRoutes.Routes
{
    [WebSocketRoute]
    public class FriendRoute : WebSocketRouteBase
    {
        private readonly FriendService friendService;
        private readonly UserService userService;
        private readonly ILogger<FriendRoute> log;

        public FriendRoute(FriendService friendService,  UserService userService, ILogger<FriendRoute> log)
        {
            this.friendService = friendService;
            this.userService = userService;
            this.log = log;
        }

        [WsAfterConnectionEstablished]
        public async Task AfterOpen()
        {
            var usrId = Claims.GetUserId().ToString();
            (await friendService.GetAll(Claims.GetUserId())).ForEach(friend =>
            {
                AddInPool(friend.Id.ToString(),usrId, UserSocket);
            });
        }

        [WsAfterConnectionClosed]
        public async Task AfterClose()
        {
            var usrId = Claims.GetUserId().ToString();
            (await friendService.GetAll(Claims.GetUserId())).ForEach(friend =>
            {
                TryRemoveFromPool(friend.Id.ToString(), usrId);
            });
        }

        [WsPost("add")]
        public async Task SendFriendRequest(Guid userId)
        {
            try
            {
                var friend = await friendService.SendFriendRequest(Claims.GetUserId(), userId);
                if (friend != null)
                {
                    var newPool = RootConnectionMannager
                        .CreateConnectionPool(friend.Id.ToString(), Claims.GetUserId().ToString(), userId.ToString());
                    await SendBack(friend, CurrentPath, Method.CONFIRM);

                    if (RootConnectionMannager.TryGetSocket(userId.ToString(), out var socket))
                    {
                        var friend2 = await this.friendService.GetById(userId, friend.Id) ?? throw new InvalidOperationException();
                        await socket.SendMessageServer(new MessageServer(CurrentPath, Method.POST, Status.Success,friend2));
                    }
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel adicionar o usuario.");
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao adicionar o usuario.");
            }
        }


        [WsPost("accept")]
        public async Task AcceptFriendRequest(Guid friendId)
        {
            try
            {
                var usrId = Claims.GetUserId();
                var friend = await friendService.AcceptFriend(usrId, friendId);
                if (friend != null)
                {
                    var pool = GetPool(friend.Id.ToString());
                    await pool.SendToAll(CurrentPath, Method.PUT, friend);
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel aceitar o pedido de amizade.");
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao aceitar o pedido de amizade.");
            }
        }

        //public record FriendMsgAdd(Guid FriendId, string content);
        //[WsRedirect("message/send")]
        //public async Task SendMessage(FriendMsgAdd add)
        //{
        //    try
        //    {
        //        add.Deconstruct(out var FriendId, out var MsgContent);
        //        var msgRess = await friendMessageService.Add(Claims.GetUserId(), FriendId, MsgContent);
        //        if (msgRess == null)
        //        {
        //            await SendErrorBack(CurrentPath, "Não foi possivel enviar a mensagem.");
        //            return;
        //        }
        //        else
        //        {
        //            await SendBack(msgRess, CurrentPath, Method.POST);
        //            var pool = GetRequiredPool(FriendId.ToString());
        //            await pool.SendToAll(CurrentPath, Method.POST, msgRess);
        //        }
        //    }
        //    catch
        //    {
        //        await SendErrorBack(CurrentPath, "Erro ao enviar a mensagem.");
        //    }
        //}

        [WsPost("reject")]
        public async Task RejectFriend(Guid FriendId)
        {
            try
            {
                var rejected = await friendService.RejectFriend(Claims.GetUserId(), FriendId);
                if (rejected)
                {
                    var pool = GetPool(FriendId.ToString());
                    await pool.SendToAll(CurrentPath, Method.DELETE, FriendId);
                    removePool(FriendId);
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel rejeitar o pedido de amizade.");
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao rejeitar o pedido de amizade.");
            }
        }



        [WsPost("cancel")]
        public async Task CalcelFriendRequest(Guid FriendId)
        {
            try
            {
                var rejected = await friendService.CancelFriendRequest(Claims.GetUserId(), FriendId);
                if (rejected)
                {
                    var pool = GetPool(FriendId.ToString());
                    await pool.SendToAll(CurrentPath, Method.DELETE, FriendId);
                    removePool(FriendId);
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel rejeitar o pedido de amizade.");
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao rejeitar o pedido de amizade.");
            }
        }

        [WsDelete]
        public async Task RemoveFriend(Guid FriendId)
        {
            try
            {
                var rejected = await friendService.ChangeToRemoved(Claims.GetUserId(), FriendId);
                if (rejected != null)
                {
                    var pool = GetPool(FriendId.ToString());
                    await pool.SendToAll(CurrentPath, Method.DELETE, FriendId);
                    removePool(FriendId);
                }
                else
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel remover o amigo.");
                }
            }
            catch
            {
                await SendErrorBack(CurrentPath, "Erro ao remover o amigo.");
            }
        }

        private void removePool(Guid FriendId)
        {
            var removed = RootConnectionMannager.TryRemoveConnectionPool(FriendId.ToString(), out _);
            if (!removed)
            {
                log.LogCritical("Unable to removed connection pool, memory issue");
            }
        }


    }

}
