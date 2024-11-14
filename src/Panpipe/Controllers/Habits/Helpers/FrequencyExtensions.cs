using Panpipe.Domain.HabitParamsSet;

namespace Panpipe.Controllers.Habits.Helpers;

public static class FrequencyExtensions
{
    public static string ToReadableString(this Frequency frequency)
    {
        return frequency.IntervalType switch
        {
            IntervalType.Day => $"Каждые (каждый) {frequency.IntervalValue} дней (дня/день)",
            _ => throw new NotImplementedException()
        };
    }
}
