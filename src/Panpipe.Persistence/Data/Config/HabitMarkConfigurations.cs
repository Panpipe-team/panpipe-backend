using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitAggregate;

namespace Panpipe.Persistence.Data.Config;

public class HabitMarkConfigurations : IEntityTypeConfiguration<HabitMark>
{
    public void Configure(EntityTypeBuilder<HabitMark> builder)
    {
        
    }
}
