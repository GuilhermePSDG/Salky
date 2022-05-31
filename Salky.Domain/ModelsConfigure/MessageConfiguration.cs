using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Salky.Domain;

namespace Salky.Domain.ModelsConfigure
{
    internal class MessageConfiguration : IEntityTypeConfiguration<MessageGroup>
    {
        public void Configure(EntityTypeBuilder<MessageGroup> builder)
        {

        }
    }
}