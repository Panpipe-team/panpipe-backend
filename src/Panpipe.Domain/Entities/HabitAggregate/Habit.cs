using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitAggregate;

public abstract class Habit<T>: AggregateRoot where T: IHabitResultType
{
    private readonly List<HabitMark<T>> habitMarks = new();

    #pragma warning disable CS8618 // Required by Entity Framework
    private Habit() {}

    public Habit(Guid id, Guid paramsId) 
    {
        Id = id;
        ParamsId = paramsId;
    }

    public IReadOnlyList<HabitMark<T>> HabitMarks => habitMarks.AsReadOnly();

    public Guid Id { get; }
    public Guid ParamsId { get; }

    public void AddEmptyMark(Guid markId, DateTimeOffset timestamp) 
    {
        if (habitMarks.Select(habitMark => habitMark.Timestamp).Contains(timestamp)) 
        {
            throw new InvalidOperationException(
                $"Can not add empty mark to habit with timestamp {timestamp}," +
                " because there is already mark with the same timestamp"
            );
        }

        habitMarks.Add(HabitMark<T>.CreateEmpty(markId, timestamp));
    }
}
