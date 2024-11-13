namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultFloat(Guid Id, float Value) : AbstractHabitResult(Id)
{
    public override HabitResultType Type => HabitResultType.Float;
}
