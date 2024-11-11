using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Presentation.Helpers;

public static class HabitResultTypeExtensions
{
    public static string GetReadable(this HabitResultType habitResultsType) {
        return habitResultsType switch
        {
            HabitResultType.Boolean => "boolean",
            HabitResultType.Float => "float",
            HabitResultType.Int => "int",
            HabitResultType.Time => "time",
            _ => throw new NotImplementedException()
        };
    }
}
