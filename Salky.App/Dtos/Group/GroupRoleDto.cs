using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Dtos.Group
{
    public class GroupRoleDto
    {
        public string RoleName { get; set; }
        public int Hierarchy { get; set; }
        public GroupPermissionsDto GroupPermissions { get; set; }
        public ChatPermissionsDto ChatPermissions { get; set; }
        public CallPermisionsDto CallPermisions { get; set; }

    }
    public class GroupPermissionsDto
    {
        public bool CanInviteOtherUsers { get; set; } = true;
        public bool CanRemoveOtherUsers { get; set; } = false;
        public bool CanEditGroupName { get; set; } = false;
        public bool CanEditGroupPicture { get; set; } = false;
        public bool CanChangeOtherUserRoles { get; set; } = false;
    }
    public class ChatPermissionsDto
    {
        public bool CanDeleteOtherUserMessages { get; set; } = false;
        public bool CanSendMessage { get; set; } = true;
        public bool CanReadMessage { get; set; } = true;
    }
    public class CallPermisionsDto
    {
        public bool CanMuteMicrofoneOfOtherUser { get; set; } = false;
        public bool CanUnMuteMicrofoneOfOtherUser { get; set; } = false;
        public bool CanMuteHeadPhoneOfOtherUser { get; set; } = false;
        public bool CanUnMuteHeadPhoneOfOtherUser { get; set; } = false;
        public bool CanEntryInCall { get; set; } = true;
        public bool CanSeeCall { get; set; } = true;
    }

}
