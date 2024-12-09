namespace Panpipe.Domain.HabitResult;

public class BooleanHabitResult(Guid id, bool value, string comment): AbstractHabitResult(id, comment)
{
    public bool Value { get; init; } = value;
    
    public override HabitResultType Type => HabitResultType.Boolean;

    public static bool TryParse(string s, string comment, out AbstractHabitResult result)
    {
        var isSuccessful = bool.TryParse(s, out var tmp);

        if (!isSuccessful)
        {
            result = new BooleanHabitResult(Guid.NewGuid(), false, comment); // Undefined hardcoded value

            return false;
        }

        result = new BooleanHabitResult(Guid.NewGuid(), tmp, comment);

        return true;
    }
}
