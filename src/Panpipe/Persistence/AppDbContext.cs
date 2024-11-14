using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Group;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;

namespace Panpipe.Persistence;

public class AppDbContext(DbContextOptions options): DbContext(options)
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<HabitParamsSet> HabitParamsSets { get; set; }
    public DbSet<AbstractHabitResult> AbstractHabitResults { get; set; }

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
            .HasForeignKey<HabitParamsSet>()
            .IsRequired();
        
        modelBuilder.Entity<HabitParamsSet>()
            .ComplexProperty(x => x.Frequency)
            .IsRequired();
        
        modelBuilder.Entity<HabitParamsSet>()
            .Property(x => x.IsPublicTemplate)
            .IsRequired();
    }
}
