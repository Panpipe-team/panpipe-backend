using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitAggregate;

public class HabitMark
{
    private HabitMark(Guid id, DateTimeOffset timestamp, IHabitResult? result) 
    {
        Id = id;
        Timestamp = timestamp;
        Result = result;
    }

    public Guid Id { get; }
    public DateTimeOffset Timestamp { get;}
    public IHabitResult? Result { get; private set; }

    public static HabitMark CreateEmpty(Guid id, DateTimeOffset timestamp)
    {
        return new HabitMark(id, timestamp, null);
    }

    public void ChangeResult(IHabitResult value) 
    {
        if (Result is IHabitResult oldValue && oldValue.Type != value.Type)
        {
            throw new InvalidOperationException
            (
                "Trying to change HabitMark value with type that differs from the old one: " +
                $"old type: {oldValue.Type}, new type: {value.Type}"
            );
        }

        Result = value;
    }
}
