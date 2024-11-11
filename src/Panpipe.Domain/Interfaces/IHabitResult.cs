using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Domain.Interfaces;

public interface IHabitResult 
{
    public HabitResultType Type { get; }
}
