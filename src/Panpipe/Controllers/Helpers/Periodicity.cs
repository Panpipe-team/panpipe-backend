using Ardalis.Result;
using Panpipe.Domain.HabitParamsSet;

namespace Panpipe.Controllers.Helpers;

public record Periodicity(string Type, int Value)
{
    public static Periodicity FromFrequency(Frequency frequency)
    {
        return new Periodicity(frequency.IntervalType.ToString(), frequency.IntervalValue);
    }

    public Result<Frequency> ToFrequency()
    {
        IntervalType? intervalType = Type switch
        {
            "Day" => IntervalType.Day,
            "Week" => IntervalType.Week,
            "Month" => IntervalType.Month,
            _ => null
        };

        if (intervalType is null)
        {
            return Result.Invalid(new ValidationError($"Wrong interval type \"{Type}\""));
        }

        if (Value <= 0)
        {
            return Result.Invalid(new ValidationError($"Interval value must be positive, not {Value}"));
        }

        return new Frequency(intervalType.Value, Value);
    }
};
