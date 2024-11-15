using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;

namespace Panpipe.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    { 
        await context.HabitParamsSets.AddRangeAsync(
            new List<HabitParamsSet> 
            {
                new (
                    Guid.NewGuid(),
                    "Выпивать воду, литров",
                    new FloatHabitResult(Guid.NewGuid(), 2.5f),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    "Отпраздновать Новый год",
                    new BooleanHabitResult(Guid.NewGuid(), true),
                    new Frequency(IntervalType.Month, 12),
                    true
                )
            }
        );

        await context.SaveChangesAsync();
    }
}
