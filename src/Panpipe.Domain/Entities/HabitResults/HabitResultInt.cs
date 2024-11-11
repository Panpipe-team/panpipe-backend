namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultInt(int Value) : AbstractHabitResult<int>(Value)
{
    public override HabitResultType Type => HabitResultType.Int;
}
