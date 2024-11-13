using Panpipe.Domain.Entities.HabitParamsAggregate;

namespace Panpipe.Presentation.Helpers;

public static class PeriodicityExtensions
{
    public static string GetReadable(this AbstractHabitPeriodicity periodicity)
    {
        return periodicity switch
        {
            DaysHabitPeriodicity days => $"Каждые (каждый) {days.IntervalValue} дней (дня/день)",
            WeeksHabitPeriodicity weeks => $"Каждые (каждую) {weeks.IntervalValue} недель (недели/неделю)",
            MonthsHabitPeriodicity months => $"Каждые (каждый) {months.IntervalValue} месяцев (месяца/месяц)",
            _ => throw new NotImplementedException()
        };
    }
}
