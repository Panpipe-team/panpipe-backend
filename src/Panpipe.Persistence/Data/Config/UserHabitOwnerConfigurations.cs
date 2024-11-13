using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitOwnerAggregate;

namespace Panpipe.Persistence.Data.Config;

public class UserHabitOwnerConfigurations : IEntityTypeConfiguration<UserHabitOwner>
{
    public void Configure(EntityTypeBuilder<UserHabitOwner> builder)
    {
        builder
            .Property(userHabitOwner => userHabitOwner.UserId);
        
        builder
            .HasOne<Habit>()
            .WithOne()
            .HasForeignKey<UserHabitOwner>(userHabitOwner => userHabitOwner.HabitId)
            .IsRequired();
    }
}
