using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Events.GroupEvents
{
    public class GroupCreated : IDomainEvent
    {
        internal GroupCreated(Group group)
        {
            Group = group;
        }

        public Group Group { get; }
    }
}