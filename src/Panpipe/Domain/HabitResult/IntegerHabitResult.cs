namespace Panpipe.Domain.HabitResult;

public class IntegerHabitResult(Guid id, int value, string comment): AbstractHabitResult(id, comment)
{
    public int Value { get; init; } = value;

    public override HabitResultType Type => HabitResultType.Integer;
    public override bool IsAchievedBy(AbstractHabitResult otherResult)
    {
        if (otherResult is IntegerHabitResult otherIntegerResult)
        {
            return otherIntegerResult.Value >= Value;
        }

        return false; // Better throw exception here or return bad result
    }

    public static bool TryParse(string s, string comment, out AbstractHabitResult result)
    {
        var isSuccessful = int.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new IntegerHabitResult(Guid.NewGuid(), 0, comment); // Undefined hardcoded value

            return false;
        }

        result = new IntegerHabitResult(Guid.NewGuid(), tmp, comment);

        return true;
    }
}