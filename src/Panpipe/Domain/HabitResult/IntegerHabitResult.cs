namespace Panpipe.Domain.HabitResult;

public class IntegerHabitResult(Guid id, int value): AbstractHabitResult(id)
{
    public int Value { get; init; } = value;

    public override HabitResultType Type => HabitResultType.Integer;

    public static bool TryParse(string s, out AbstractHabitResult result)
    {
        var isSuccessful = int.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new IntegerHabitResult(Guid.NewGuid(), 0); // Undefined hardcoded value

            return false;
        }

        result = new IntegerHabitResult(Guid.NewGuid(), tmp);

        return true;
    }
}