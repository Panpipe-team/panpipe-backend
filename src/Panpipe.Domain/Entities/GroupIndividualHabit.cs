namespace Panpipe.Domain.Entities;

public class GroupIndividualHabit : Habit
{
    public GroupIndividualHabit() { }

    public GroupIndividualHabit(
        Guid id,
        string name,
        HabitResultType habitResultType,
        string goal,
        Frequency frequency,
        Account account,
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
        Account = account;
        Group = group;
        HabitType = HabitType.GroupIndividual;
    }
    public virtual required Account Account { get; set; }
    public virtual required Group Group { get; set; }
}