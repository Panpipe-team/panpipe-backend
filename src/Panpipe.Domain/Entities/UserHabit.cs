namespace Panpipe.Domain.Entities;

public class UserHabit : Habit
{
    public UserHabit() { }

    public UserHabit(
        Guid id,
        string name,
        HabitResultType habitResultType,
        string goalData,
        Frequency frequency,
        Account account,
        string? description = null)
        : base(
            id,
            name,
            habitResultType,
            goalData,
            frequency,
            description)
    {
        Account = account;
        HabitType = HabitType.Personal;
    }
    public virtual required Account Account { get; set; }
}