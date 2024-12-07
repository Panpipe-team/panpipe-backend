namespace Panpipe.Domain.HabitResult;

public class FloatHabitResult(Guid id, float value): AbstractHabitResult(id)
{
    public float Value { get; init; } = value;

    public override HabitResultType Type => HabitResultType.Float;

    public static bool TryParse(string s, out AbstractHabitResult result)
    {
        var isSuccessful = float.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new FloatHabitResult(Guid.NewGuid(), 0); // Undefined hardcoded value

            return false;
        }

        result = new FloatHabitResult(Guid.NewGuid(), tmp);

        return true;
    }
}