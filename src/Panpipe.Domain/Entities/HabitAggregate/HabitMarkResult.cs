using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitAggregate;

public class HabitMarkResult<T> where T: IHabitResultType
{
    public HabitMarkResult(T value) {
        Value = value;
    }

    public T Value { get; }
}
