namespace Panpipe.Domain.HabitResult;

public abstract class AbstractHabitResult(Guid id, string? comment)
{
    public Guid Id { get; init; } = id;
    public string? Comment { get; init; } = comment;
    public abstract HabitResultType Type { get; }
}
