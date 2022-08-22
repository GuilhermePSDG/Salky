using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Salky.App.Services.Group;
using Salky.App.Storage;
using Salky.WebSockets.Exensions;

namespace Salky.API.WebSocketRoutes
{

    [WebSocketRoute("group/call")]
    public class CalRoute : WebSocketRouteBaseCustom
    {
        private UserCall? ___usrCall;
        private UserCall CurrentUserCall => ___usrCall ??= this.UserStorage.GetOrCreate("call:state", () => UserCall.Default(this.User.UserId));
        private Call? CurrentCall => CurrentUserCall.IsInCall ? RootStorage.GetOrCreate($"call:{CurrentUserCall.CallId}", () => new Call()) : null;

        private readonly ILogger<CalRoute> logger;
        private readonly GroupMemberService groupMemberService;
        public CalRoute(ILogger<CalRoute> logger, GroupMemberService groupMemberService)
        {
            this.logger = logger;
            this.groupMemberService = groupMemberService;
        }
        public override async Task OnDisconnectAsync()
        {
            if (CurrentUserCall.IsInCall)
                await __leaveCall();
            await base.OnDisconnectAsync();
        }
        #region Entry or leave
        public record EntryCallObject(Guid GroupId);
        [WsPost]
        public async Task EntryCall(EntryCallObject EntryCallObject)
        {
            EntryCallObject.Deconstruct(out var GroupId);
            if (!await groupMemberService.UserMakePartOfGroup(Guid.Parse(User.UserId), GroupId))
            {
                await SendErrorBack(CurrentPath, "User do not make part of group", CurrentRouteMethod);
                return;
            }
            if (CurrentUserCall.IsInCall)
            {
                await this.__leaveCall();
            }
            CurrentUserCall.CallId = GroupId.ToString();
            CurrentCall!.UsersInCall.Add(this.User.UserId, this.CurrentUserCall);
            await AddOneInPool($"call:{CurrentUserCall.CallId}", this.User.UserId);
            await SendToAllInPool(GroupId, new(CurrentRoutePath.Path, CurrentRoutePath.Method, Status.Success, this.CurrentUserCall));
        }
        [WsDelete]
        public async Task LeaveCall()
        {
            if (!CurrentUserCall.IsInCall)
            {
                await SendErrorBack(CurrentPath, "User is not in a call", CurrentRouteMethod);
                return;
            }
            await __leaveCall();
        }
        private async Task __leaveCall()
        {
            var GroupId = CurrentUserCall.CallId;
            CurrentCall!.UsersInCall.Remove(this.User.UserId);
            await RemoveOneFromPool($"call:{CurrentUserCall.CallId}", this.User.UserId);
            await SendToAllInPool(GroupId, new("group/call", Method.DELETE, Status.Success, this.CurrentUserCall));
            CurrentUserCall.CallId = null;
        }
        #endregion


        [WsGet("all")]
        public async Task AllMembers(Guid GroupId)
        {
            try
            {
                if (!await this.groupMemberService.UserMakePartOfGroup(Guid.Parse(this.User.UserId), GroupId))
                    return;

                var _call = RootStorage.Get($"call:{GroupId}");
                if (_call is Call call)
                {
                    var usrs = call.UsersInCall.Values.Where(x => x != null && x.IsInCall)
                    .ToList();
                    await SendBack(usrs, CurrentPath, CurrentRouteMethod);
                }
                else
                {
                    await SendBack(new List<UserCall>(), CurrentPath, CurrentRouteMethod);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "");
            }
        }

        #region Audio


        [WsRedirect]
        public async Task SendAudio(object data)
        {
            if (!CurrentUserCall.IsInCall)
            {
                await SendErrorBack(CurrentPath, "User is not in a call", CurrentRouteMethod);
                return;
            }
            if (!CurrentUserCall.AudioState.CanSpeak)
            {
                await SendErrorBack(CurrentPath, "User can not speak", CurrentRouteMethod);
                return;
            }

            var filtredCallMembers = CurrentCall!.UsersInCall.Values
                .Where(x => x != null && x.IsInCall && x.AudioState.CanHear && x.UserId != this.User.UserId)
                .Select(x => (Key)x.UserId)
                .ToArray();
            await SendToManyInPool($"call:{CurrentUserCall.CallId}", filtredCallMembers, new MessageServer(CurrentPath, CurrentRouteMethod, Status.Success, data));
        }

        #endregion

        [WsPut]
        public void UpdateAudioState(AudioState audioState)
        {
            CurrentUserCall.AudioState = audioState;
            if (!CurrentUserCall.IsInCall) return;
            SendToAllInPool(CurrentUserCall.CallId, new(CurrentPath, CurrentRouteMethod, Status.Success, CurrentUserCall));
        }
    }

    public class Call
    {
        public Dictionary<string, UserCall> UsersInCall = new();
    }

    public class UserCall
    {
        public AudioState AudioState { get; set; }
        public string UserId { get; set; }

        private string? _callId = null;

        public UserCall(string UserId, AudioState audio)
        {
            this.UserId = UserId;
            AudioState = audio;
        }
        public string CallId
        {
            get => _callId!;
            set => _callId = value;
        }
        public bool IsInCall { get => _callId != null; set { } }
        public static UserCall Default(string UserId) => new(UserId, new AudioState(false, false));
    }

    public class AudioState
    {
        public AudioState(bool microfoneMuted, bool headsetMuted)
        {
            MicroFoneMuted = microfoneMuted;
            HeadPhoneMuted = headsetMuted;
        }
        public AudioState()
        {

        }
        public bool MicroFoneMuted
        {
            get => ___MicrofoneMuted;
            set
            {
                if (!value)
                    HeadPhoneMuted = false;
                ___MicrofoneMuted = value;

            }
        }
        public bool HeadPhoneMuted
        {
            get => ___HeadsetMuted;
            set
            {
                if (value)
                    MicroFoneMuted = true;
                ___HeadsetMuted = value;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        private bool ___MicrofoneMuted;
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        private bool ___HeadsetMuted;
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public bool CanSpeak => !MicroFoneMuted && !HeadPhoneMuted;
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public bool CanHear => !HeadPhoneMuted;
    }

}
