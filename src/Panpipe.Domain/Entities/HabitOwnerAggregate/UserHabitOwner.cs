namespace Panpipe.Domain.Entities.HabitOwnerAggregate;

public class UserHabitOwner: AbstractHabitOwner
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid HabitId { get; }

    public UserHabitOwner(Guid id, Guid userId, Guid habitId): base()
    {
        Id = id;
        UserId = userId;
        HabitId = habitId;
    }
}
