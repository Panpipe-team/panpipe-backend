namespace Panpipe.Domain.HabitOwner;

public class GroupHabitOwner(Guid id, Guid groupId, Guid habitId)
{
    public Guid Id { get; init; } = id;
    public Guid GroupId { get; init; } = groupId;
    public Guid HabitId { get; init; } = habitId;
}
