using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;
using Panpipe.Domain.Tags;

namespace Panpipe.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    { 
        await context.Tags.AddRangeAsync(
            new List<Tag>
            {
                new (Guid.NewGuid(), "Полезная привычка"),
                new (Guid.NewGuid(), "Вредная привычка")
            }
        );

        await context.HabitParamsSets.AddRangeAsync(
            new List<HabitParamsSet> 
            {
                new (
                    Guid.NewGuid(),
                    "Выпивать воду, литров",
                    "Описание",
                    [],
                    new FloatHabitResult(Guid.NewGuid(), 2.5f, null),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    "Отпраздновать день рождения",
                    "Описание",
                    [],
                    new BooleanHabitResult(Guid.NewGuid(), true, null),
                    new Frequency(IntervalType.Month, 12),
                    true
                )
            }
        );

        await context.SaveChangesAsync();
    }
}
