using Panpipe.Domain.Entities.HabitResults;
using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitParamsAggregate;

public class HabitParams: AggregateRoot
{
    #pragma warning disable CS8618 // Required by Entity Framework
    private HabitParams() {}

    public HabitParams
    (
        Guid id, 
        string name, 
        IHabitPeriodicity periodicity, 
        IHabitResult goal, 
        bool isPublicTemplate
    ): this() 
    {
        Id = id;
        Name = name;
        Periodicity = periodicity;
        Goal = goal;
        IsPublicTemplate = isPublicTemplate;
    }

    public Guid Id { get; }
    public string Name { get; private set;}
    public IHabitPeriodicity Periodicity { get; private set; }
    public IHabitResult Goal { get; private set; }
    public bool IsPublicTemplate { get; }

    public HabitResultType ResultType => Goal.Type;

    public bool HasSameResultType(IHabitResult result) => ResultType == result.Type;

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
