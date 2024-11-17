namespace Panpipe.Domain.HabitOwner;

public class UserHabitOwner(Guid id, Guid userId, Guid habitId)
{
    public Guid Id { get; init; } = id;
    public Guid UserId { get; init; } = userId;
    public Guid HabitId { get; init; } = habitId;
}
