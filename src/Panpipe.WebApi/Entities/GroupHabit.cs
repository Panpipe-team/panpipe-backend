namespace Panpipe.WebApi.Entities;

public class GroupHabit : Habit
{
    public GroupHabit() { }

    public GroupHabit(
        Guid id,
        string name,
        HabitResultType habitResultType,
        string goal,
        Frequency frequency,
        Group group,
        string? description = null)
        : base(
            id,
            name,
            habitResultType,
            goal,
            frequency,
            description)
    {
        Group = group;
        HabitType = HabitType.GroupShared;
    }

    public virtual Group Group { get; set; }
}