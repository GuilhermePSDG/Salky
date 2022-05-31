using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Events.GroupEvents
{
    public class GroupDeleted : IDomainEvent
    {
        internal GroupDeleted(Group group)
        {
            Group = group;
        }

        public Group Group { get; }
    }
}