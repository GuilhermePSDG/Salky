

using AutoMapper;
using Salky.API.WebSocketRoutes.Models;
using Salky.App.Services.Group;
using Salky.WebSocket.Infra.Interfaces;

namespace Salky.API.WebSocketRoutes.Routes
{


    [WebSocketRoute("group/call")]
    public class CallRoute : WebSocketRouteBase
    {
        //
        private readonly GroupMemberService groupMemberService;
        public CallRoute(GroupMemberService groupMemberService)
        {
            this.groupMemberService = groupMemberService;
        }
        [WsAfterConnectionClosed]
        public async Task AfterClose()
        {
            try
            {
                var usrCall = Storage.Get<GroupMemberCall>();
                if (usrCall.IsInCall && usrCall.CurrentCallPath != null)
                {
                    if(RootConnectionMannager.TryGetConnectionPool(usrCall.CurrentCallPath,out var pool))
                        await pool.SendToAll(
                            path: $"group/call",
                            method: Method.DELETE,
                            data: usrCall
                            );
                    usrCall.ZeroCallProperties();
                }
            }
            finally
            {

            }
        }
        [WsPost]
        public async Task EntryOrCreateCall(CallEntry callEntry)
        {
            try
            {
                var groupid = callEntry.GroupId.ToString();
                var member = await this.groupMemberService.GetMemberWithRole(Claims.GetUserId(),callEntry.GroupId);
                if (member == null)
                {
                    await SendErrorBack(CurrentPath, "Usuario não pode entrar nesse grupo");
                    return;
                }
                if (!member.GroupRole.CallPermisions.CanEntryInCall)
                {
                    await SendErrorBack(CurrentPath, "Usuario não pode entrar nesse grupo");
                    return;
                }

                var groupPool = GetPool(groupid);
                var usrCall = Storage.Get<GroupMemberCall>();
                if (usrCall.IsInCall)
                {
                    await QuitFromCall();
                }
                //Atualiza o usuario
                usrCall.FillCallProperties(groupid,groupid, member.Id.ToString(),member.GroupRole);
                usrCall.AudioState = callEntry.AudioState;
                //Faz o envio do usuario conectado, para os outros
                await groupPool.SendToAll(
                 path: CurrentPath,
                 method: Method.POST,
                 data: usrCall
                 );
            }
            catch (Exception ex)
            {
                await SendErrorBack(CurrentPath, "Erro ao entrar na chamada " + ex.Message);
            }
        }
        [WsDelete]
        public async Task QuitFromCall()
        {
            try
            {
                //Recupera o usuario e verifica se está em uma call
                var usrCall = Storage.Get<GroupMemberCall>();
                if (!usrCall.IsInCall)
                {
                    await SendErrorBack(CurrentPath, "Usuario não está em uma chamada");
                    return;
                }
                //Recupera a pool da call
                var pool = GetPool(usrCall.CurrentCallPath ?? throw new InvalidOperationException());
                if (pool == null)
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel recuperar a chamada.");
                    return;
                }
                //Faz o envio da notificação para quem interessar
                await pool.SendToAll(
                    path: CurrentPath,
                    method: Method.DELETE,
                    data: usrCall
                    );
                //Zera as propiedades que indicam em qual call os usuario está
                usrCall.ZeroCallProperties();
            }
            catch (Exception ex)
            {
                await SendErrorBack(CurrentPath, "Erro ao sair da chamada " + ex.Message);
            }
        }
        [WsRedirect]
        public async Task SendAudio(string data)
        {
            try
            {
                var usr = Storage.Get<GroupMemberCall>();
                if (!usr.IsInCall || usr.CurrentCallPath == null || usr.GroupId == null)
                {
                    await SendErrorBack(CurrentPath, "Usuario não está em uma chamada");
                    return;
                }
                if (usr.AudioState.CanTalk)
                {
                    await SendErrorBack(CurrentPath, "Usuario está mutado.");
                    return;
                }
                var connectionPool = GetPool(usr.CurrentCallPath);
                if (connectionPool == null)
                {
                    await SendErrorBack(CurrentPath, "Não foi possivel encontrar a chamada.");
                    try
                    {
                        await this.QuitFromCall();
                    }
                    finally
                    {
                        usr.ZeroCallProperties();
                    }

                    return;
                }
                //Faz o envio do audio
                await connectionPool.SendToAll(
                    CanSendToThis: socket =>
                    {
                        var otherUser = socket.Storage.Get<GroupMemberCall>();
                        return 
                        otherUser.IsInCall &&
                        otherUser.GroupId == usr.GroupId &&
                        otherUser.AudioState.CanHear &&
                        socket.UniqueId != UserSocket.UniqueId
                        ;
                    },
                    path: CurrentPath,
                    method: Method.REDIRECT,
                    data: data
                    );
            }
            catch
            {
                await SendErrorBack("group/call/audio", "Erro ao enviar o audio");
            }
        }
        [WsGet("all")]
        public async Task GetUsersOfCall(Guid GroupId)
        {
            if (await groupMemberService.UserMakePartOfGroup(Claims.GetUserId(), GroupId))
            {
                var groupId = GroupId.ToString();
                var groupPool = GetPool(groupId);
                var users = groupPool
                    .WhereSocket(sock => sock.ConnectionsIsOpen && sock.Storage.Has<GroupMemberCall>())
                    .Select(x => x.Storage.Get<GroupMemberCall>())
                    .Where(usr => usr.IsInCall && usr.GroupId == groupId)
                    .ToList();
                await SendBack(users, CurrentPath, Method.GET_RESPONSE);
            }
            else
            {
                await SendErrorBack(CurrentPath, "Usuario não tem acceso nesta chamada.");
            }
        }
        [WsGet]
        public async Task GetSelfUser() => await SendBack(Storage.Get<GroupMemberCall>(), CurrentPath, Method.GET_RESPONSE);
        [WsPut]
        public async Task UpdateAudioInfo(AudioState userUiInfo)
        {
            var usrCall = Storage.Get<GroupMemberCall>();
            usrCall.AudioState = userUiInfo;
            if (usrCall.IsInCall && usrCall.GroupId != null)
                await GetPool(usrCall.GroupId).SendToAll(CurrentPath, Method.PUT, usrCall);
        }

    }
}
