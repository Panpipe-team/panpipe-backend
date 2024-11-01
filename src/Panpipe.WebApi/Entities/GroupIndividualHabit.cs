namespace Panpipe.WebApi.Entities;

public class GroupIndividualHabit : Habit
{
    public GroupIndividualHabit() { }

    public GroupIndividualHabit(
        Guid id,
        string name,
        HabitResultType habitResultType,
        string goal,
        Frequency frequency,
        User user,
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
        User = user;
        Group = group;
        HabitType = HabitType.GroupIndividual;
    }
    public virtual User User { get; set; }
    public virtual Group Group { get; set; }
}