using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Events.GroupEvents
{
    public class GroupMemberAdded : IDomainEvent
    {
        internal GroupMemberAdded(GroupMember member)
        {
            Member = member;
        }

        public GroupMember Member { get; }
    }
}