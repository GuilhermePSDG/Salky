using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Contexts
{
    internal class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder
                .HasOne(f => f.Group)
                .WithMany(f => f.Members);
            builder
                .HasOne(f => f.User)
                .WithMany(f => f.Groups);
            builder
                .HasOne(x => x.Role)
                .WithMany(x => x.MemberWithRoles)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}