namespace Panpipe.Domain.HabitOwner;

public class GroupUserHabitOwner(Guid id, Guid userId, Guid groupId, Guid habitId)
{
    public Guid Id { get; init; } = id;
    public Guid UserId { get; init; } = userId;
    public Guid GroupId { get; init; } = groupId;
    public Guid HabitId { get; init; } = habitId;
}
