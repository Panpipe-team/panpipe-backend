using Panpipe.Domain.HabitParamsSet;

namespace Panpipe.Controllers.Habits.Helpers;

public static class HabitTypeExtensions
{
    public static string ToReadableString(this HabitType habitType)
    {
        return habitType switch
        {
            HabitType.Health => "Здоровье",
            HabitType.Sport => "Спорт",
            HabitType.SelfDevelopment => "Саморазвитие",
            _ => throw new NotImplementedException()
        };
    }
}