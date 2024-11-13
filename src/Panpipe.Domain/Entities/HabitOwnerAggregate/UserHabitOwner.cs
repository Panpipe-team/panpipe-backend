namespace Panpipe.Domain.Entities.HabitOwnerAggregate;

public class UserHabitOwner: AbstractHabitOwner
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid HabitId { get; init; }

    private UserHabitOwner() { }
    
    public UserHabitOwner(Guid id, Guid userId, Guid habitId): base()
    {
        Id = id;
        UserId = userId;
        HabitId = habitId;
    }
}
