using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;

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
        await context.HabitParamsSets.AddRangeAsync(
            new List<HabitParamsSet> 
            {
                new (
                    Guid.NewGuid(),
                    $"Пройти {countSteps} шагов в день",
                    new IntegerHabitResult(Guid.NewGuid(), countSteps),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Спать {hoursSleep} часов в день",
                    new IntegerHabitResult(Guid.NewGuid(), hoursSleep),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Заниматься уроками английского языка {countEnglishLesson} раза в неделю",
                    new IntegerHabitResult(Guid.NewGuid(), countEnglishLesson),
                    new Frequency(IntervalType.Week, 3),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Прочитать {countPage} страниц в день",
                    new IntegerHabitResult(Guid.NewGuid(), countPage),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Рабочая сессия без отвлечения {hoursJobSession} часов в день",
                    new IntegerHabitResult(Guid.NewGuid(), hoursJobSession),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Потратить {spendCalories} калорий в день",
                    new FloatHabitResult(Guid.NewGuid(), spendCalories),
                    new Frequency(IntervalType.Day, 1),
                    false
                ),
            }
        );

        await context.SaveChangesAsync();
    }
}