using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities;

namespace Panpipe.Persistence.Data.Config;

public class GroupConfigurations : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .Property(group => group.Name);
        
        builder
            .Property(group => group.UserIds);
            // .HasConversion(
            //     v => v.Select(guid => guid.ToString()),
            //     v => v.Select(Guid.Parse).ToList()
            // );
    }
}
