using Salky.Domain.Models.GenericsModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salky.Domain.Models.GroupModels
{
    public class GroupRole : BaseEntity
    {
        public string RoleName { get; set; }
        public int Hierarchy { get; set; }
        public Group Group { get; set; }
        public Guid GroupId { get; set; }


        public virtual CallPermisions CallPermisions { get; set; }
        public virtual ChatPermissions ChatPermissions { get; set; }
        public virtual GroupPermissions GroupPermissions { get; set; }
        //

        public virtual List<GroupMember> MemberWithRoles { get; set; } = new List<GroupMember>();

        private GroupRole()
        {

        }

        private GroupRole(string roleName, int hierarchy,Guid Groupid):this(roleName,hierarchy)
        {
            this.GroupId = Groupid; 
        }
        private GroupRole(string roleName, int hierarchy) : this()
        {
            RoleName = roleName;
            Hierarchy = hierarchy;
        }
        public static GroupRole Default() => new("Default", int.MaxValue)
        {
            CallPermisions = CallPermisions.Default(),
            ChatPermissions = ChatPermissions.Default(),
            GroupPermissions = GroupPermissions.Default()
        };



        public static GroupRole Admin() => new("Admin", 1)
        {
            CallPermisions = CallPermisions.Admin(),
            ChatPermissions = ChatPermissions.Admin(),
            GroupPermissions = GroupPermissions.Admin()
        };

        

    }

    public class GroupPermissions : BaseEntity
    {
        public GroupRole GroupRole { get; set; }
        [ForeignKey(nameof(GroupRole))]
        public Guid GroupRoleId { get; set; }

        private GroupPermissions()
        {

        }
        public static GroupPermissions Default() => new GroupPermissions();
        public static GroupPermissions Admin() => new GroupPermissions()
        {
            CanChangeOtherUserRoles = true,
            CanEditGroupName = true,
            CanEditGroupPicture = true,
            CanInviteOtherUsers = true,
            CanRemoveOtherUsers = true,
        };
        public bool CanInviteOtherUsers { get; set; } = true;
        public bool CanRemoveOtherUsers { get; set; } = false;
        public bool CanEditGroupName { get; set; } = false;
        public bool CanEditGroupPicture { get; set; } = false;
        public bool CanChangeOtherUserRoles { get; set; } = false;
    }
    public class ChatPermissions : BaseEntity
    {
        public GroupRole GroupRole { get; set; }
        [ForeignKey(nameof(GroupRole))]
        public Guid GroupRoleId { get; set; }


        public static ChatPermissions Default() => new ChatPermissions();
        public static ChatPermissions Admin() => new ChatPermissions()
        {
            CanDeleteOtherUserMessages = true,
            CanReadMessage = true,
            CanSendMessage = true,
        };
        public bool CanDeleteOtherUserMessages { get; set; } = false;
        public bool CanSendMessage { get; set; } = true;
        public bool CanReadMessage { get; set; } = true;
    }
    public class CallPermisions : BaseEntity
    {
        public GroupRole GroupRole { get; set; }
        [ForeignKey(nameof(GroupRole))]
        public Guid GroupRoleId { get; set; }

        public static CallPermisions Default() => new CallPermisions();
        public static CallPermisions Admin() => new CallPermisions()
        {
            CanEntryInCall = true,
            CanMuteHeadPhoneOfOtherUser = true,
            CanMuteMicrofoneOfOtherUser = true,
            CanSeeCall = true,
            CanUnMuteHeadPhoneOfOtherUser = true,
            CanUnMuteMicrofoneOfOtherUser = true,
        };

        public bool CanMuteMicrofoneOfOtherUser { get; set; } = false;
        public bool CanUnMuteMicrofoneOfOtherUser { get; set; } = false;
        public bool CanMuteHeadPhoneOfOtherUser { get; set; } = false;
        public bool CanUnMuteHeadPhoneOfOtherUser { get; set; } = false;
        public bool CanEntryInCall { get; set; } = true;
        public bool CanSeeCall { get; set; } = true;
    }



}