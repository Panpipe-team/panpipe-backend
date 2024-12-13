using Panpipe.Domain.HabitResult;

namespace Panpipe.Controllers.Habits.Helpers;

public static class AbstractHabitResultExtensions
{
    public static string ToReadableString(this AbstractHabitResult habitResult)
    {
        return habitResult switch
        {
            BooleanHabitResult booleanHabitResult => booleanHabitResult.Value.ToString(),
            FloatHabitResult floatHabitResult => floatHabitResult.Value.ToString(),
            IntegerHabitResult integerHabitResult => integerHabitResult.Value.ToString(),
            TimeHabitResult timeHabitResult => timeHabitResult.Value.ToString(),
            _ => throw new NotImplementedException()
        };
    }
}
