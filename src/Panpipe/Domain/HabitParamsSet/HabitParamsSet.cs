using Ardalis.Result;
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
    public AbstractHabitResult Goal { get; private set; }
    public Frequency Frequency { get; init; }
    public bool IsPublicTemplate { get; init; }

    public ICollection<Tag> Tags => _tags.AsReadOnly();
    public HabitResultType ResultType => Goal.Type;

    public void SetName(string name) => Name = name;
    public void SetDescription(string description) => Description = description;
    public Result SetGoal(AbstractHabitResult newGoal)
    {
        if (Goal.Type != newGoal.Type)
        {
            return Result.Invalid(new ValidationError(
                $"New goal has type {newGoal.Type}, while old one has type {Goal.Type}, which is incompatible"
            ));
        }

        Goal = newGoal;

        return Result.Success();
    }


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
