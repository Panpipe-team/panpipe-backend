namespace Panpipe.Domain.HabitResult;

public static class HabitResultTypeExtensions
{
    public static bool TryParse(this HabitResultType resultType, string s, out AbstractHabitResult result)
    {
        return resultType switch
        {
            HabitResultType.Boolean => BooleanHabitResult.TryParse(s, out result),
            HabitResultType.Float => FloatHabitResult.TryParse(s, out result),
            HabitResultType.Integer => IntegerHabitResult.TryParse(s, out result),
            HabitResultType.Time => TimeHabitResult.TryParse(s, out result),
            _ => throw new NotImplementedException()
        };
    }
}
