

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
        private readonly ILogger<CallRoute> logger;

        public CallRoute(GroupMemberService groupMemberService,ILogger<CallRoute> logger)
        {
            this.groupMemberService = groupMemberService;
            this.logger = logger;
        }
        [WsAfterConnectionClosed]
        public async Task AfterClose()
        {
            try
            {
                var usrCall = Storage.Get<GroupMemberCall>();
                if (usrCall.IsInCall && usrCall.PoolPath != null)
                {
                    await SendToAllInPool(usrCall.PoolPath, "group/call", Method.DELETE, usrCall);
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
                var callMember = Storage.Get<GroupMemberCall>();
                if (callMember.IsInCall) await QuitFromCall();
                var poolPath = $"{groupid}/call";
                callMember.FillCallProperties(groupid, poolPath, member.Id.ToString(), member.GroupRole);
                callMember.AudioState = callEntry.AudioState;
                AddInPool(poolPath, member.UserId.ToString());
                await SendToAllInPool(groupid, CurrentPath, Method.POST, callMember);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao entrar na chamada");
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
                TryRemoveFromPool(usrCall.PoolPath ?? throw new NullReferenceException(), Claims.GetUserId().ToString());
                //Faz o envio da notificação para quem interessar
                await SendToAllInPool(
                    PoolId:usrCall.GroupId ?? throw new NullReferenceException(),
                    Path: CurrentPath,
                    method: Method.DELETE,
                    data: usrCall
                    );
                //Zera as propiedades que indicam em qual call os usuario está
                usrCall.ZeroCallProperties();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao sair da chamada");
                await SendErrorBack(CurrentPath, "Erro ao sair da chamada " + ex.Message);
            }
        }
        [WsRedirect]
        public async Task SendAudio(string data)
        {
            try
            {
                var usr = Storage.Get<GroupMemberCall>();
                if (!usr.IsInCall || usr.PoolPath == null || usr.GroupId == null)
                {
                    await SendErrorBack(CurrentPath, "Usuario não está em uma chamada");
                    return;
                }
                if (!usr.AudioState.CanTalk)
                {
                    await SendErrorBack(CurrentPath, "Usuario está mutado.");
                    return;
                }

                var usrId = Claims.GetUserId().ToString();
                foreach(var storage in RootConnectionMannager.GetStorageOfManyInPool(usr.PoolPath))
                {
                    var callState = storage.Value.Get<GroupMemberCall>();
                    if (storage.Key != usrId && callState.AudioState.CanHear)
                        await SendToOneInPool(usr.PoolPath,storage.Key, CurrentPath, Method.REDIRECT, data);
                }

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Erro ao enviar o audio");
                await SendErrorBack("group/call/audio", "Erro ao enviar o audio");
            }
        }
        [WsGet("all")]
        public async Task GetUsersOfCall(Guid GroupId)
        {
            if (await groupMemberService.UserMakePartOfGroup(Claims.GetUserId(), GroupId))
            {
                var poolPath = $"{GroupId}/call";
                var usrs = RootConnectionMannager
                    .GetStorageOfManyInPool(poolPath)
                    .Select(x => x.Value.Get<GroupMemberCall>())
                    .ToList();
                await SendBack(usrs, CurrentPath, Method.GET_RESPONSE);
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
                await SendToAllInPool(usrCall.GroupId.ToString(), CurrentPath, Method.PUT, usrCall);
            Storage.AddOrUpdate(usrCall);
        }

    }
}
