using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitAggregate;

public class UserHabit<T>: AbstractHabit<T> where T: IHabitResultType
{
    public UserHabit(Guid id, Guid paramsId, Guid userId) : base(id, paramsId)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}