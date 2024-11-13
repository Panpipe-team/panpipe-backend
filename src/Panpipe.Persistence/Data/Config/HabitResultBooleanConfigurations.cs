using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Persistence.Data.Config;

public class HabitResultBooleanConfigurations : IEntityTypeConfiguration<HabitResultBoolean>
{
    public void Configure(EntityTypeBuilder<HabitResultBoolean> builder)
    {
        builder
            .Property(habitResult => habitResult.Value);
    }
}
