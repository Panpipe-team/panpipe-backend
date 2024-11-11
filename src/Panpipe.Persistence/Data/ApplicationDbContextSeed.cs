using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Persistence.Data;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.HabitParams.AddRangeAsync(
            new List<HabitParams> 
            {
                new (
                    Guid.NewGuid(),
                    "Выпивать воду, литров",
                    new DaysHabitPeriodicity(1),
                    new HabitResultFloat(2.5f),
                    true
                )
            }
        );
        
        await context.SaveChangesAsync();
    }
}