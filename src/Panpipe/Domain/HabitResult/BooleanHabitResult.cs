namespace Panpipe.Domain.HabitResult;

public class BooleanHabitResult(Guid id, bool value): AbstractHabitResult(id)
{
    public bool Value { get; init; } = value;
    
    public override HabitResultType Type => HabitResultType.Boolean;

    public static bool TryParse(string s, out AbstractHabitResult result)
    {
        var isSuccessful = bool.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new BooleanHabitResult(Guid.NewGuid(), false); // Undefined hardcoded value

            return false;
        }

        result = new BooleanHabitResult(Guid.NewGuid(), tmp);

        return true;
    }
}
