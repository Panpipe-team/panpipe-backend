using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Persistence.Data.Config;

public class HabitParamsConfigurations : IEntityTypeConfiguration<HabitParams>
{
    public void Configure(EntityTypeBuilder<HabitParams> builder)
    {
        builder
            .Property(habitParams => habitParams.Name);

        builder
            .HasOne(habitParams => habitParams.Periodicity)
            .WithOne()
            .HasForeignKey<HabitParams>("HabitPeriodicityId")
            .IsRequired();

        builder
            .HasOne(habitParams => habitParams.Goal)
            .WithOne()
            .HasForeignKey<HabitParams>("HabitResultGoalId")
            .IsRequired();
        
        builder
            .Property(habitParams => habitParams.IsPublicTemplate);
    }
}
