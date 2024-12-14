namespace Panpipe.Domain.HabitResult;

public static class HabitResultTypeExtensions
{
    public static bool TryParse(
        this HabitResultType resultType, string s, string comment, out AbstractHabitResult result
    )
    {
        return resultType switch
        {
            HabitResultType.Boolean => BooleanHabitResult.TryParse(s, comment, out result),
            HabitResultType.Float => FloatHabitResult.TryParse(s, comment, out result),
            HabitResultType.Integer => IntegerHabitResult.TryParse(s, comment, out result),
            HabitResultType.Time => TimeHabitResult.TryParse(s, comment, out result),
            _ => throw new NotImplementedException()
        };
    }
}
