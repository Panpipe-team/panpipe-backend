namespace Panpipe.Domain.Entities.HabitResults;

public record HabitResultTime(TimeOnly Value) : AbstractHabitResult<TimeOnly>(Value)
{
    public override HabitResultType Type => HabitResultType.Time;
} 
