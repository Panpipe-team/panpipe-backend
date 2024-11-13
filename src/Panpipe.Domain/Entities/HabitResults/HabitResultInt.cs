namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultInt(Guid Id, int Value) : AbstractHabitResult(Id)
{
    public override HabitResultType Type => HabitResultType.Int;
}
