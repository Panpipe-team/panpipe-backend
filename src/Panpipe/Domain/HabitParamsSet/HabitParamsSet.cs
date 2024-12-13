using Panpipe.Domain.HabitResult;

namespace Panpipe.Domain.HabitParamsSet;

public class HabitParamsSet
{
    #pragma warning disable CS8618 // Required by Entity Framework
    private HabitParamsSet() {}

    public HabitParamsSet(Guid id, string name, AbstractHabitResult goal, Frequency frequency, bool isPublicTemplate, HabitType habitType)
    {
        Id = id;
        Name = name;
        Goal = goal;
        Frequency = frequency;
        IsPublicTemplate = isPublicTemplate;
        HabitType = habitType;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public HabitType HabitType { get; private set; }
    
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
