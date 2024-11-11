using Panpipe.Domain.Entities.HabitResults;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Application.Helpers;

public static class HabitResultTypeExtensions
{
    public static bool TryParse(this HabitResultType habitResultType, string s, out IHabitResult result)
    {
        return habitResultType switch 
        {
            HabitResultType.Boolean => TryParseBool(s, out result),
            HabitResultType.Float => TryParseFloat(s, out result),
            HabitResultType.Int => TryParseInt(s, out result),
            HabitResultType.Time => TryParseTime(s, out result),
            _ => throw new NotImplementedException()
        };
    }

    private static bool TryParseBool(string s, out IHabitResult result)
    {
        var isSuccessful = bool.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new HabitResultBoolean(false); // Undefined hardcoded value

            return false;
        }

        result = new HabitResultBoolean(tmp);

        return true;
    }

    private static bool TryParseInt(string s, out IHabitResult result)
    {
        var isSuccessful = int.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new HabitResultInt(0); // Undefined hardcoded value

            return false;
        }

        result = new HabitResultInt(tmp);

        return true;
    }

    private static bool TryParseFloat(string s, out IHabitResult result)
    {
        var isSuccessful = float.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new HabitResultFloat(0); // Undefined hardcoded value

            return false;
        }

        result = new HabitResultFloat(tmp);

        return true;
    }

    private static bool TryParseTime(string s, out IHabitResult result)
    {
        var isSuccessful = TimeOnly.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new HabitResultTime(TimeOnly.FromDateTime(DateTime.UtcNow)); // Undefined hardcoded value

            return false;
        }

        result = new HabitResultTime(tmp);

        return true;
    }
}
