using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Domain.Entities.HabitParamsAggregate;

public class HabitParams: AggregateRoot
{
    #pragma warning disable CS8618 // Required by Entity Framework
    private HabitParams() {}

    public HabitParams
    (
        Guid id, 
        string name, 
        AbstractHabitPeriodicity periodicity, 
        AbstractHabitResult goal, 
        bool isPublicTemplate
    ): this() 
    {
        Id = id;
        Name = name;
        Periodicity = periodicity;
        Goal = goal;
        IsPublicTemplate = isPublicTemplate;
    }

    public Guid Id { get; init; }
    public string Name { get; private set;}
    public AbstractHabitPeriodicity Periodicity { get; private set; }
    public AbstractHabitResult Goal { get; private set; }
    public bool IsPublicTemplate { get; init; }

    public HabitResultType ResultType => Goal.Type;

    public bool HasSameResultType(AbstractHabitResult result) => ResultType == result.Type;

    public List<DateTimeOffset> CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit() 
        => Periodicity.CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

    public List<DateTimeOffset> CaluclateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp
    (
        DateTimeOffset lastMarkTimestamp
    ) 
        => Periodicity.CaluclateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp
        (
            lastMarkTimestamp
        );
}
