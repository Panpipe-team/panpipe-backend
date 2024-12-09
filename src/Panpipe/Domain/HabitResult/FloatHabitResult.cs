namespace Panpipe.Domain.HabitResult;

public class FloatHabitResult(Guid id, float value, string comment): AbstractHabitResult(id, comment)
{
    public float Value { get; init; } = value;

    public override HabitResultType Type => HabitResultType.Float;

    public static bool TryParse(string s, string comment, out AbstractHabitResult result)
    {
        var isSuccessful = float.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new FloatHabitResult(Guid.NewGuid(), 0, comment); // Undefined hardcoded value

            return false;
        }

        result = new FloatHabitResult(Guid.NewGuid(), tmp, comment);

        return true;
    }
}
