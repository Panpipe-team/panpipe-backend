using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitAggregate;

public class UserHabitOwner(Guid userId): IHabitOwner
{
    public Guid UserId { get; } = userId;
}
