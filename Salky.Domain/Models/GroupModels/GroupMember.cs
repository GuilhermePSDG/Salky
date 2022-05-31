using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.UserModels;

namespace Salky.Domain.Models.GroupModels
{
    public class GroupMember : BaseEntity
    {
        public GroupMember() : base() { }
        private GroupMember(Guid GroupId, Guid UserId, GroupRole Role) : this()
        {
            (this.GroupId, this.UserId, this.Role, RoleId) = (GroupId, UserId, Role, Role.Id);
        }

        public static GroupMember Create(Guid GroupId, Guid UserId, GroupRole Role)
        {
            return new GroupMember(GroupId, UserId, Role);
        }

        public User User { get; set; }
        public Guid UserId { get; set; }
        public Group Group { get; set; }
        public Guid GroupId { get; set; }
        public virtual GroupRole Role { get; set; }
        public Guid RoleId { get; set; }

        /// <summary>
        /// I never lie, i swear 😁
        /// </summary>
        /// <param name="messageGroup"></param>
        /// <returns><see langword="true"/> if can, otherwise <see langword="false"/> </returns>
        public bool ICanDeleteThisMessage(MessageGroup messageGroup)
        {
            if (messageGroup.SenderId == UserId) return true;
            if (Role.ChatPermissions.CanDeleteOtherUserMessages) return true;
            return false;
        }

    }
}
