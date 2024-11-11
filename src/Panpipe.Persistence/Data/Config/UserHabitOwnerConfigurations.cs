using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitAggregate;

namespace Panpipe.Persistence.Data.Config;

public class HabitConfigurations : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        
    }
}
