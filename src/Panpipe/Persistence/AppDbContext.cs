using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Group;
using Panpipe.Domain.Habit;
using Panpipe.Domain.HabitOwner;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;

namespace Panpipe.Persistence;

public class AppDbContext(DbContextOptions options): DbContext(options)
{
    #pragma warning disable CS8618 

    public DbSet<Group> Groups { get; set; }
    public DbSet<HabitParamsSet> HabitParamsSets { get; set; }
    public DbSet<AbstractHabitResult> AbstractHabitResults { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitMark> HabitMarks { get; set; }
    public DbSet<UserHabitOwner> UserHabitOwners { get; set; }

    #pragma warning restore CS8618

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Group
        modelBuilder.Entity<Group>()
            .Property(x => x.Name)
            .IsRequired();

        modelBuilder.Entity<Group>()
            .Property(x => x.UserIds)
            .IsRequired();
        
        // AbstractHabitResult
        modelBuilder.Entity<AbstractHabitResult>()
            .UseTpcMappingStrategy();

        // BooleanHabitResult
        modelBuilder.Entity<BooleanHabitResult>()
            .Property(x => x.Value)
            .IsRequired();

        // FloatHabitResult
        modelBuilder.Entity<FloatHabitResult>()
            .Property(x => x.Value)
            .IsRequired();
            
        // HabitParamsSet
        modelBuilder.Entity<HabitParamsSet>()
            .Property(x => x.Name)
            .IsRequired();
        
        modelBuilder.Entity<HabitParamsSet>()
            .HasOne(x => x.Goal)
            .WithOne()
            .HasForeignKey<HabitParamsSet>("habitResultAsGoalId")
            .IsRequired();
        
        modelBuilder.Entity<HabitParamsSet>()
            .ComplexProperty(x => x.Frequency)
            .IsRequired();
        
        modelBuilder.Entity<HabitParamsSet>()
            .Property(x => x.IsPublicTemplate)
            .IsRequired();
        
        // Habit
        modelBuilder.Entity<Habit>()
            .HasOne<HabitParamsSet>()
            .WithMany()
            .HasForeignKey(x => x.ParamsSetId)
            .IsRequired();
        
        modelBuilder.Entity<Habit>()
            .HasMany(x => x.Marks)
            .WithOne();
        
        // HabitMark
        modelBuilder.Entity<HabitMark>()
            .Property(x => x.TimestampUtc)
            .IsRequired();

        modelBuilder.Entity<HabitMark>()
            .HasOne(x => x.Result)
            .WithOne()
            .HasForeignKey<HabitMark>("resultId")
            .IsRequired(false);
        
        // UserHabitOwner        
        modelBuilder.Entity<UserHabitOwner>()
            .Property(x => x.UserId)
            .IsRequired();

        modelBuilder.Entity<UserHabitOwner>()
            .HasOne<Habit>()
            .WithOne()
            .HasForeignKey<UserHabitOwner>(x => x.HabitId)
            .IsRequired();
    }
}