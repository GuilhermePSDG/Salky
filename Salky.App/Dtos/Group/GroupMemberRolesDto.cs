using Salky.Domain.Models.GroupModels;

namespace Salky.App.Dtos.Group
{
    public class GroupMemberRolesDto : GroupMemberDto
    {
        public GroupRole GroupRole { get; set; }
        public Guid UserId { get; set; }
    }
}
