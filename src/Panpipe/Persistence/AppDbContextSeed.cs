using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;
using Panpipe.Domain.Tags;

namespace Panpipe.Persistence;

public static class AppDbContextSeed
{
    private static int countSteps = 5000;
    private static int hoursSleep = 7;
    private static int countEnglishLesson = 3;
    private static int countPage = 30;
    private static int hoursJobSession = 8;
    private static float spendCalories = 200.5F;
    
    public static async Task SeedAsync(AppDbContext context)
    {
        var usefulTag = new Tag(Guid.NewGuid(), "Полезная привычка");
        var harmfulTag = new Tag(Guid.NewGuid(), "Вредная привычка");
        var sportTag = new Tag(Guid.NewGuid(), "Спорт");
        await context.Tags.AddRangeAsync(
            new List<Tag>
            {
                usefulTag,
                harmfulTag,
                sportTag,
            }
        );
        await context.HabitParamsSets.AddRangeAsync(
            new List<HabitParamsSet> 
            {
                new (
                    Guid.NewGuid(),
                    "Выпивать воду, литров",
                    "Описание",
                    [usefulTag],
                    new FloatHabitResult(Guid.NewGuid(), 2.5f, null),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    "Отпраздновать день рождения",
                    "Описание",
                    [usefulTag],
                    new BooleanHabitResult(Guid.NewGuid(), true, null),
                    new Frequency(IntervalType.Month, 12),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Пройти {countSteps} шагов в день",
                    "", [usefulTag, sportTag],
                    new IntegerHabitResult(Guid.NewGuid(), countSteps, null),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Заниматься уроками английского языка по 15 минут, каждые два дня",
                    "", [usefulTag],
                    new TimeHabitResult(Guid.NewGuid(), new TimeSpan(0, 15, 0), null),
                    new Frequency(IntervalType.Day, 2),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Рабочая сессия без отвлечения {hoursJobSession} часов в день",
                    "", [usefulTag],
                    new IntegerHabitResult(Guid.NewGuid(), hoursJobSession, null),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Потратить {spendCalories} калорий в день",
                    "", [usefulTag, sportTag],
                    new FloatHabitResult(Guid.NewGuid(), spendCalories, ""),
                    new Frequency(IntervalType.Day, 1),
                    false
                ),
            }
        );

        await context.SaveChangesAsync();
    }
}
