using Panpipe.Domain.Entities.HabitParamsAggregate;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Presentation.Helpers;

public static class PeriodicityExtensions
{
    public static string GetReadable(this IHabitPeriodicity periodicity)
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
