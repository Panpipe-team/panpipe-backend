namespace Panpipe.Domain.HabitResult;

public class TimeHabitResult(Guid id, TimeSpan value): AbstractHabitResult(id)
{
    public TimeSpan Value { get; init; } = value;

    public override HabitResultType Type => HabitResultType.Time;

    public static bool TryParse(string s, out AbstractHabitResult result)
    {
        var isSuccessful = TimeSpan.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new TimeHabitResult(Guid.NewGuid(), default); // Undefined hardcoded value

            return false;
        }

        result = new TimeHabitResult(Guid.NewGuid(), tmp);

        return true;
    }
}