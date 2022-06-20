using Salky.App.Dtos.Group;
using Salky.Domain.Models.GroupModels;

namespace Salky.API.WebSocketRoutes.Models
{
    public class GroupMemberCall
    {
        public GroupMemberCall(AudioState audioState)
        {
            AudioState = audioState;
        }
        public string? MemberId { get; set; } = null;
        public bool IsInCall { get;private set; } = false;
        public string? GroupId { get; private set; } = null;
        public AudioState AudioState { get;  set; }


        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public GroupRole? Roles { get; private set; }
        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public string? PoolPath { get; private set; } = null;

        /// <summary>
        /// Todas as propiedades relacionadas a call serão setadas para null ou false
        /// </summary>
        public void ZeroCallProperties()
        {
            this.PoolPath = null;
            this.IsInCall = false;
            this.GroupId = null;
            this.MemberId = null;
            this.Roles = null;
        }
        public void FillCallProperties(string GroupId, string PoolPath, string CurrentCallMemberId,GroupRole roles)
        {
            this.IsInCall = true;
            this.PoolPath = PoolPath;
            this.GroupId = GroupId;
            this.MemberId = CurrentCallMemberId;
            this.Roles = roles;
        }


    }
}
