using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Persistence.Data.Config;

public class AbstractHabitResultConfigurations : IEntityTypeConfiguration<AbstractHabitResult>
{
    public void Configure(EntityTypeBuilder<AbstractHabitResult> builder)
    {
        builder
            .UseTpcMappingStrategy();
    }
}
