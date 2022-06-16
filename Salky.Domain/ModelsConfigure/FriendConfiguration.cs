using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salky.Domain.Models.FriendModels;


namespace Salky.Domain.Contexts
{
    internal class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.HasOne(a => a.RequestedBy)
                .WithMany(b => b.SentFriendRequests)
                .HasForeignKey(c => c.RequestedById);
            builder.HasOne(a => a.RequestedTo)
                .WithMany(b => b.ReceievedFriendRequests)
                .HasForeignKey(c => c.RequestedToId);
        }
    }
}
