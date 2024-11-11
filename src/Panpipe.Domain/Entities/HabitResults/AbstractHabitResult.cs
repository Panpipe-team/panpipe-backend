using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitResults;

public abstract record AbstractHabitResult<T>(T Value): IHabitResult 
{
    public abstract HabitResultType Type { get;}
}
