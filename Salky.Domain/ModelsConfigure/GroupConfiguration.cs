using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salky.Domain.Models.GroupModels;

namespace Salky.Domain.Contexts
{
    internal class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder
                .HasMany(x => x.Members)
                .WithOne(x => x.Group);
            builder
                .HasOne(x => x.Owner)
                .WithMany(x => x.OwnerGroups);
            builder
                .HasMany(x => x.GroupRoles)
                .WithOne(x => x.Group);
        }
    }
}