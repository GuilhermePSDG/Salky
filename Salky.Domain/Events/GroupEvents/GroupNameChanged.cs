using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Events.GroupEvents
{
    public class GroupNameChanged : FieldChanged<string>, IDomainEvent
    {
        internal GroupNameChanged(Guid GroupId, string NewName) : base(GroupId, NewName, nameof(Group.Name))
        {

        }
    }
}