using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Persistence.Data.Config;

public class HabitResultIntConfigurations : IEntityTypeConfiguration<HabitResultInt>
{
    public void Configure(EntityTypeBuilder<HabitResultInt> builder)
    {
        builder
            .Property(habitResult => habitResult.Value);        
    }
}
