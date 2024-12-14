namespace Panpipe.Domain.HabitResult;

public class TimeHabitResult(Guid id, TimeSpan value, string comment): AbstractHabitResult(id, comment)
{
    public TimeSpan Value { get; init; } = value;

    public override HabitResultType Type => HabitResultType.Time;
    public override bool IsAchievedBy(AbstractHabitResult otherResult)
    {
        if (otherResult is TimeHabitResult otherTimeResult)
        {
            return otherTimeResult.Value >= Value;
        }

        return false; // Better throw exception here or return bad result
    }

    public static bool TryParse(string s, string comment, out AbstractHabitResult result)
    {
        var isSuccessful = TimeSpan.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new TimeHabitResult(Guid.NewGuid(), default, comment); // Undefined hardcoded value

            return false;
        }

        result = new TimeHabitResult(Guid.NewGuid(), tmp, comment);

        return true;
    }
}