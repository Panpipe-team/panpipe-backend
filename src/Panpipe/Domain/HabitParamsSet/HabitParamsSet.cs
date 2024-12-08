using Panpipe.Domain.HabitResult;

namespace Panpipe.Domain.HabitParamsSet;

public class HabitParamsSet
{
    #pragma warning disable CS8618 // Required by Entity Framework
    private HabitParamsSet() {}

    public HabitParamsSet(
        Guid id, string name, string description, AbstractHabitResult goal, Frequency frequency, bool isPublicTemplate
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Goal = goal;
        Frequency = frequency;
        IsPublicTemplate = isPublicTemplate;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public AbstractHabitResult Goal { get; init; }
    public Frequency Frequency { get; init; }
    public bool IsPublicTemplate { get; init; }

    public HabitResultType ResultType => Goal.Type;

    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit() 
        => Frequency.CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp(
        DateTimeOffset lastMarkTimestamp
    ) 
        => Frequency.CalculateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp
        (
            lastMarkTimestamp
        );
}
