using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Domain.Entities.HabitAggregate;

public class HabitMark
{
    private HabitMark() { }

    private HabitMark(Guid id, DateTimeOffset timestamp, AbstractHabitResult? result) 
    {
        Id = id;
        Timestamp = timestamp;
        Result = result;
    }

    public Guid Id { get; init; }
    public DateTimeOffset Timestamp { get; init;}
    public AbstractHabitResult? Result { get; private set; }

    public static HabitMark CreateEmpty(Guid id, DateTimeOffset timestamp)
    {
        return new HabitMark(id, timestamp, null);
    }

    public void ChangeResult(AbstractHabitResult value) 
    {
        if (Result is AbstractHabitResult oldValue && oldValue.Type != value.Type)
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
