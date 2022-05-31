namespace Salky.App.Dtos.Group
{
    public class GroupMemberRolesDto : GroupMemberDto
    {
        public GroupRoleDto GroupRole { get; set; }
        public Guid UserId { get; set; }
    }
}
