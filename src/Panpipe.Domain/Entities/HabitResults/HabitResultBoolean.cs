namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultBoolean(bool Value) : AbstractHabitResult<bool>(Value)
{
    public override HabitResultType Type => HabitResultType.Boolean;
}
