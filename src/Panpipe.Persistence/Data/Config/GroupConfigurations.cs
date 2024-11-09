using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities;

namespace Panpipe.Persistence.Data.Config;

public class GroupConfigurations : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            // .HasKey(group => group.Id)
            .Property(group => group.Name)
            .IsRequired();
    }
}
