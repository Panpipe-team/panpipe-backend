namespace Panpipe.Domain.Entities.HabitResults;

public abstract record AbstractHabitResult(Guid Id) 
{
    public abstract HabitResultType Type { get; }
}
