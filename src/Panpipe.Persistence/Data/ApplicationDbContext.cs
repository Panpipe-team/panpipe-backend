using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Panpipe.Domain.Entities;
using Panpipe.Domain.Entities.HabitAggregate;
using Panpipe.Domain.Entities.HabitOwnerAggregate;
using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Persistence.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {}

    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitMark> HabitMarks { get; set; }
    public DbSet<HabitResultBoolean> HabitResultBooleans { get; set; }
    public DbSet<HabitResultFloat> HabitResultFloats { get; set; }
    public DbSet<HabitResultInt> HabitResultInts { get; set; }
    public DbSet<HabitResultTime> HabitResultTimes { get; set; }    
    public DbSet<HabitParams> HabitParams { get; set; }
    public DbSet<UserHabitOwner> UserHabitOwners { get; set; }
    public DbSet<Group> Groups { get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
