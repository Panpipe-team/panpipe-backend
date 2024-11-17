namespace Panpipe.Domain.HabitResult;

public abstract class AbstractHabitResult(Guid id)
{
    public Guid Id { get; init; } = id;
    public abstract HabitResultType Type { get; }
}
