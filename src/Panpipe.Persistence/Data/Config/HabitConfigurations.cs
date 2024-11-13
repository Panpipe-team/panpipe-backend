using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Persistence.Data.Config;

public class HabitConfigurations : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder
            .HasOne<HabitParams>()
            .WithMany()
            .HasForeignKey(habit => habit.ParamsId)
            .IsRequired();
        
        builder
            .HasMany(habit => habit.HabitMarks)
            .WithOne()
            .IsRequired();
    }
}
