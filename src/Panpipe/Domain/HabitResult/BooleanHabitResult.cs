namespace Panpipe.Domain.HabitResult;

public class BooleanHabitResult(Guid id, bool value): AbstractHabitResult(id)
{
    public bool Value { get; init; } = value;
    
    public override HabitResultType Type => HabitResultType.Boolean;
}
