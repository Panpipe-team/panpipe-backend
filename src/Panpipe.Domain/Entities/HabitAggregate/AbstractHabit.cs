using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitAggregate;

public abstract class AbstractHabit<T>: AggregateRoot, IHabit where T: IHabitResultType
{
    private readonly List<HabitMark<T>> habitMarks = new();

    #pragma warning disable CS8618 // Required by Entity Framework
    private AbstractHabit() {}

    public AbstractHabit(Guid id, Guid paramsId) 
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

    public void ChangeResult(Guid markId, T result) 
    {
        var mark = habitMarks.FirstOrDefault(habitMark => habitMark.Id == markId) 
            ?? throw new InvalidOperationException($"Cannot change habit result: no mark with id {markId}");

        mark.ChangeResult(result);
    }
}
