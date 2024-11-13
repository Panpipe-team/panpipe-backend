namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultTime(Guid Id, TimeOnly Value) : AbstractHabitResult(Id)
{
    public override HabitResultType Type => HabitResultType.Time;
} 
