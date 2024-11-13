namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultBoolean(Guid Id, bool Value): AbstractHabitResult(Id)
{
    public override HabitResultType Type => HabitResultType.Boolean;
}
