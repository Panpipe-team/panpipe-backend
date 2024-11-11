namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultFloat(float Value) : AbstractHabitResult<float>(Value)
{
    public override HabitResultType Type => HabitResultType.Float;
}
