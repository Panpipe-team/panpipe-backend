using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Group;
using Panpipe.Domain.Habit;
using Panpipe.Domain.HabitCollection;
using Panpipe.Domain.HabitOwner;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;
using Panpipe.Domain.Tags;

namespace Panpipe.Persistence;

public class AppDbContext(DbContextOptions options): DbContext(options)
{
    #pragma warning disable CS8618 

    public DbSet<Group> Groups { get; set; }
    public DbSet<HabitParamsSet> HabitParamsSets { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<AbstractHabitResult> AbstractHabitResults { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitCollection> HabitCollections { get; set; }
    public DbSet<HabitMark> HabitMarks { get; set; }
    public DbSet<UserHabitOwner> UserHabitOwners { get; set; }
    public DbSet<GroupHabitOwner> GroupHabitOwners { get; set; }
    public DbSet<GroupUserHabitOwner> GroupUserHabitOwners { get; set; }

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
        
        modelBuilder.Entity<BooleanHabitResult>()
            .Property(x => x.Comment)
            .IsRequired(false);

        // FloatHabitResult
        modelBuilder.Entity<FloatHabitResult>()
            .Property(x => x.Value)
            .IsRequired();
        
        modelBuilder.Entity<FloatHabitResult>()
            .Property(x => x.Comment)
            .IsRequired(false);

        // Tag
        modelBuilder.Entity<Tag>()
            .Property(x => x.Name)
            .IsRequired();
            
        // HabitParamsSet
        modelBuilder.Entity<HabitParamsSet>()
            .Property(x => x.Name)
            .IsRequired();

        modelBuilder.Entity<HabitParamsSet>()
            .Property(x => x.Description)
            .IsRequired();

        modelBuilder.Entity<HabitParamsSet>()
            .HasMany(x => x.Tags)
            .WithMany();
        
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
        
        // HabitCollection
        modelBuilder.Entity<HabitCollection>()
            .HasOne<HabitParamsSet>()
            .WithMany()
            .HasForeignKey(x => x.ParamsSetId)
            .IsRequired();
        
        modelBuilder.Entity<HabitCollection>()
            .Property(x => x.HabitIds)
            .IsRequired();
        
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
        
        // GroupHabitOwner        
        modelBuilder.Entity<GroupHabitOwner>()
            .HasOne<Group>()
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .IsRequired();

        modelBuilder.Entity<GroupHabitOwner>()
            .HasOne<Habit>()
            .WithOne()
            .HasForeignKey<GroupHabitOwner>(x => x.HabitId)
            .IsRequired();
        
        // GroupUserHabitOwner        
        modelBuilder.Entity<GroupUserHabitOwner>()
            .Property(x => x.UserId)
            .IsRequired();
        
        modelBuilder.Entity<GroupUserHabitOwner>()
            .HasOne<Group>()
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .IsRequired();

        modelBuilder.Entity<GroupUserHabitOwner>()
            .HasOne<Habit>()
            .WithOne()
            .HasForeignKey<GroupUserHabitOwner>(x => x.HabitId)
            .IsRequired();
    }
}
