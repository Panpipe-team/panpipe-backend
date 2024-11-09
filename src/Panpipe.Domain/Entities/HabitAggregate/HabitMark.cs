using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitAggregate;

public class HabitMark<T> where T: IHabitResultType
{
    private HabitMark(Guid id, DateTimeOffset timestamp, HabitMarkResult<T>? result) 
    {
        Id = id;
        Timestamp = timestamp;
        Result = result;
    }

    public Guid Id { get; }
    public DateTimeOffset Timestamp { get;}
    public HabitMarkResult<T>? Result { get; private set; }

    public static HabitMark<T> CreateEmpty(Guid id, DateTimeOffset timestamp)
    {
        return new HabitMark<T>(id, timestamp, null);
    }

    public void ChangeResult(T value) 
    {
        Result = new HabitMarkResult<T>(value);
    }
}
