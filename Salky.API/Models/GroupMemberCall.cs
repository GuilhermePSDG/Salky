using Salky.App.Dtos.Group;
using Salky.Domain.Models.GroupModels;

namespace Salky.API.Models
{
    public class GroupMemberCall
    {
        public GroupMemberCall(AudioState audioState)
        {
            AudioState = audioState;
        }
        public string? MemberId { get; set; } = null;
        public bool IsInCall { get; private set; } = false;
        public string? GroupId { get; private set; } = null;
        public AudioState AudioState { get; set; }


        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public GroupRole? Roles { get; private set; }
        [Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
        public string? PoolPath { get; private set; } = null;


        public void ZeroCallProperties()
        {
            PoolPath = null;
            IsInCall = false;
            GroupId = null;
            MemberId = null;
            Roles = null;
        }
        public void FillCallProperties(string GroupId, string PoolPath, string CurrentCallMemberId, GroupRole roles)
        {
            IsInCall = true;
            this.PoolPath = PoolPath;
            this.GroupId = GroupId;
            MemberId = CurrentCallMemberId;
            Roles = roles;
        }


    }
}
