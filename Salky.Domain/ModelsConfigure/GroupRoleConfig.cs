using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.ModelsConfigure
{
    internal class GroupRoleConfig : IEntityTypeConfiguration<GroupRole>
    {
        public void Configure(EntityTypeBuilder<GroupRole> builder)
        {
            builder
                .HasOne(x => x.Group)
                .WithMany(x => x.GroupRoles);

            builder
                .HasMany(x => x.MemberWithRoles)
                .WithOne(x => x.Role);

            builder
                .HasIndex(x => new {
                    x.RoleName,
                    x.GroupId}
                )
                .IsUnique();
            
        }
    }
}