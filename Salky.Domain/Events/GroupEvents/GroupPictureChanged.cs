using Salky.Domain.Contracts;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Events.GroupEvents
{
    public class GroupPictureChanged : FieldChanged<string>, IDomainEvent
    {
        internal GroupPictureChanged(Guid GroupId, string PictureSource) : base(GroupId, PictureSource, nameof(Group.PictureSource))
        {
        }
    }


}
