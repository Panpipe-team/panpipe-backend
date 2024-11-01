namespace Panpipe.WebApi.Entities;

public class UserHabit : Habit
{
    public UserHabit() { }

    public UserHabit(
        Guid id,
        string name,
        HabitResultType habitResultType,
        string goalData,
        Frequency frequency,
        User user,
        string? description = null)
        : base(
            id,
            name,
            habitResultType,
            goalData,
            frequency,
            description)
    {
        User = user;
        HabitType = HabitType.Personal;
    }
    public virtual required User User { get; set; }
}