using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Persistence.Data.Config;

public class HabitResultFloatConfigurations : IEntityTypeConfiguration<HabitResultFloat>
{
    public void Configure(EntityTypeBuilder<HabitResultFloat> builder)
    {
        builder
            .Property(habitResult => habitResult.Value);        
    }
}
