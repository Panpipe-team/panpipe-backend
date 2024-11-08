using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitResults;

public abstract class AbstractHabitResultType<T>: IHabitResultType
{
    public AbstractHabitResultType(T value) 
    {
        Value = value;
    }
    
    public T Value { get; }
}