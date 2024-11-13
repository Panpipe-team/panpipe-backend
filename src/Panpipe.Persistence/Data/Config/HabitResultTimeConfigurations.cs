using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Persistence.Data.Config;

public class HabitResultTimeConfigurations : IEntityTypeConfiguration<HabitResultTime>
{
    public void Configure(EntityTypeBuilder<HabitResultTime> builder)
    {
        builder
            .Property(habitResult => habitResult.Value);        
    }
}
