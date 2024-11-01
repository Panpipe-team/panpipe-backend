namespace Panpipe.WebApi.Entities;

public abstract class Habit
{
    protected Habit(
        Guid id,
        string name,
        HabitResultType habitResultType,
        string goal,
        Frequency frequency,
        string? description = null)
    {
        Id = id;
        Name = name;
        Description = description;
        HabitResultType = habitResultType;
        Goal = goal;
        Frequency = frequency;
    }

    protected Habit() { }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public HabitType HabitType { get; set; }

    public string? Description { get; set; }

    public HabitResultType HabitResultType { get; set; }

    public string Goal { get; set; }

    public Frequency Frequency { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
