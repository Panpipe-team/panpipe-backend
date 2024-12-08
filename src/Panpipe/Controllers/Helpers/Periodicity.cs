using Panpipe.Domain.HabitParamsSet;

namespace Panpipe.Controllers.Helpers;

public record Periodicity(string Type, int Value)
{
    public static Periodicity FromFrequency(Frequency frequency)
    {
        return new Periodicity(frequency.IntervalType.ToString(), frequency.IntervalValue);
    }
};
