namespace Panpipe.Domain.Entities.HabitResults;

public class HabitResultTime: AbstractHabitResultType<TimeOnly>
{
    public HabitResultTime(TimeOnly value): base(value) {}
}
