using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Domain.Entities.HabitAggregate;

public class Habit: AggregateRoot
{
    private readonly List<HabitMark> habitMarks = new();

    #pragma warning disable CS8618 // Required by Entity Framework
    private Habit() {}

    public Habit(Guid id, Guid paramsId) 
    {
        Id = id;
        ParamsId = paramsId;
    }

    public IReadOnlyList<HabitMark> HabitMarks => habitMarks.AsReadOnly();

    public Guid Id { get; init; }
    public Guid ParamsId { get; init; }

    public void AddEmptyMark(Guid markId, DateTimeOffset timestamp) 
    {
        if (habitMarks.Select(habitMark => habitMark.Timestamp).Contains(timestamp)) 
        {
            throw new InvalidOperationException(
                $"Can not add empty mark to habit with timestamp {timestamp}," +
                " because there is already mark with the same timestamp"
            );
        }

        habitMarks.Add(HabitMark.CreateEmpty(markId, timestamp));
    }

    public void ChangeResult(Guid markId, AbstractHabitResult result) 
    {
        var mark = habitMarks.FirstOrDefault(habitMark => habitMark.Id == markId) 
            ?? throw new InvalidOperationException($"Cannot change habit result: no mark with id {markId}");

        mark.ChangeResult(result);
    }
}
