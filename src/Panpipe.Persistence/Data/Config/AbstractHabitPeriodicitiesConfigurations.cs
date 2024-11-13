using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Persistence.Data.Config;

public class AbstractHabitPeriodicitiesConfigurations : 
    IEntityTypeConfiguration<AbstractHabitPeriodicity>
{
    public void Configure(EntityTypeBuilder<AbstractHabitPeriodicity> builder)
    {
        builder
            .UseTphMappingStrategy();
    }
}
