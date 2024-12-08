using Panpipe.Domain.HabitResult;
using Panpipe.Domain.Tags;

namespace Panpipe.Domain.HabitParamsSet;

public class HabitParamsSet
{
    private readonly List<Tag> _tags;

    #pragma warning disable CS8618 // Required by Entity Framework
    private HabitParamsSet() {}

    public HabitParamsSet(
        Guid id, 
        string name, 
        string description, 
        List<Tag> tags, 
        AbstractHabitResult goal, 
        Frequency frequency, 
        bool isPublicTemplate
    )
    {
        Id = id;
        Name = name;
        Description = description;
        _tags = tags;
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

    public ICollection<Tag> Tags => _tags.AsReadOnly();
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
