namespace Panpipe.WebApi.Entities;

public class HabitTemplate
{
    public HabitTemplate() { }

    public HabitTemplate(
        Guid id,
        string name,
        HabitResultType habitResultType,
        string goalData,
        Frequency frequency,
        string? description = null)
    {
        Id = id;
        Name = name;
        Description = description;
        HabitResultType = habitResultType;
        GoalData = goalData;
        Frequency = frequency;
    }

    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public HabitResultType HabitResultType { get; set; }

    public required string GoalData { get; set; }

    public required Frequency Frequency { get; set; }
}