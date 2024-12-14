using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;
using Panpipe.Domain.Tags;

namespace Panpipe.Persistence;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context)
    {
        const string EmptyHabitGoalComment = "";

        var popularTag = new Tag(Guid.NewGuid(), "Популярное");
        var healthTag = new Tag(Guid.NewGuid(), "Здоровье");
        var sportTag = new Tag(Guid.NewGuid(), "Спорт");
        var selfImprovementTag = new Tag(Guid.NewGuid(), "Саморазвитие");
        var activityTag = new Tag(Guid.NewGuid(), "Активность");
        var comfortTag = new Tag(Guid.NewGuid(), "Комфорт");
        var productivityTag = new Tag(Guid.NewGuid(), "Продуктивность");
        var progressTag = new Tag(Guid.NewGuid(), "Прогресс");
        var longTermGoalsTag = new Tag(Guid.NewGuid(), "Долгосрочные цели");

        await context.Tags.AddRangeAsync(
            new List<Tag>
            {
                popularTag,
                healthTag,
                sportTag,
                selfImprovementTag,
                activityTag,
                comfortTag,
                productivityTag,
                progressTag,
                longTermGoalsTag
            }
        );

        const float waterLitersPerDay = 2.0f;
        const int stepsPerDay = 10000;
        const int englishLessonsDurationMinutesPerTwoDays = 15;
        const int caloriesAmountSpentPerDay = 300;
        const int monthsPeriodBetweenDentistAppointment = 3;
        const int monthsPeriodBetweenMedicalCheckups = 12;

        await context.HabitParamsSets.AddRangeAsync(
            new List<HabitParamsSet> 
            {
                new (
                    Guid.NewGuid(),
                    $"Выпить {waterLitersPerDay} литра воды за день",
                    $"Сделай привычкой выпивать {waterLitersPerDay} литра воды ежедневно, " +
                    "чтобы заботиться о своём теле и разуме. " +
                    "Эта простая практика поддерживает иммунитет, помогает избежать усталости и улучшает общий тонус.",
                    [popularTag, healthTag],
                    new FloatHabitResult(Guid.NewGuid(), waterLitersPerDay, EmptyHabitGoalComment),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Пройти {stepsPerDay} шагов за день",
                    $"Проходи {stepsPerDay} шагов каждый день, чтобы оставаться активным и поддерживать здоровье. " +
                    "Этот простой шаг сделает тебя ближе к лучшей форме и самочувствию", 
                    [activityTag],
                    new IntegerHabitResult(Guid.NewGuid(), stepsPerDay, EmptyHabitGoalComment),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Заниматься уроками английского языка по {englishLessonsDurationMinutesPerTwoDays} " +
                    "минут каждые два дня",
                    "Делай шаг к изучению английского каждые два дня, уделяя всего " +
                    $"{englishLessonsDurationMinutesPerTwoDays} минут времени. " +
                    "Такой подход сделает процесс комфортным, а прогресс — заметным и стабильным.", 
                    [selfImprovementTag],
                    new TimeHabitResult(Guid.NewGuid(), new TimeSpan(0, 15, 0), EmptyHabitGoalComment),
                    new Frequency(IntervalType.Day, 2),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Потратить {caloriesAmountSpentPerDay} калорий за день",
                    $"Ежедневно трать {caloriesAmountSpentPerDay} калорий, занимаясь любимыми активностями " +
                    "или упражнениями. " +
                    "Это отличный способ укрепить здоровье и сделать шаг к лучшему самочувствию.", 
                    [activityTag],
                    new FloatHabitResult(Guid.NewGuid(), caloriesAmountSpentPerDay, EmptyHabitGoalComment),
                    new Frequency(IntervalType.Day, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Убираться в комнате раз в неделю",
                    "Делай уборку в комнате каждую неделю, чтобы сохранять чистоту и улучшать качество воздуха. " +
                    "Регулярное поддержание порядка способствует твоему физическому и mental здоровью.", 
                    [comfortTag],
                    new BooleanHabitResult(Guid.NewGuid(), true, EmptyHabitGoalComment),
                    new Frequency(IntervalType.Week, 1),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Посещать стоматолога раз в {monthsPeriodBetweenDentistAppointment} месяца",
                    "Уделяй внимание своему здоровью и посещай стоматолога каждые " +
                    $"{monthsPeriodBetweenDentistAppointment} месяца. Это важная привычка для предотвращения кариеса " +
                    "и других заболеваний зубов, которая обеспечит твою улыбку на долгие годы.", 
                    [popularTag, healthTag],
                    new BooleanHabitResult(Guid.NewGuid(), true, EmptyHabitGoalComment),
                    new Frequency(IntervalType.Month, 3),
                    true
                ),
                new (
                    Guid.NewGuid(),
                    $"Проходить диспансеризацию раз в {monthsPeriodBetweenMedicalCheckups} месяцев",
                    $"Запланируй диспансеризацию раз в {monthsPeriodBetweenMedicalCheckups} месяцев, " +
                    "чтобы контролировать состояние своего здоровья. " +
                    "Это важная профилактическая мера, которая помогает своевременно обнаружить возможные заболевания.",
                    [healthTag],
                    new BooleanHabitResult(Guid.NewGuid(), true, EmptyHabitGoalComment),
                    new Frequency(IntervalType.Month, 12),
                    true
                ),
            }
        );

        await context.SaveChangesAsync();
    }
}
