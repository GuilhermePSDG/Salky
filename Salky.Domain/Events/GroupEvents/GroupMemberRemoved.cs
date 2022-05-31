using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Events.GroupEvents
{
    public class GroupMemberRemoved : IDomainEvent
    {
        public GroupMember groupMember;

        internal GroupMemberRemoved(GroupMember groupMember)
        {
            this.groupMember = groupMember;
        }
    }
}