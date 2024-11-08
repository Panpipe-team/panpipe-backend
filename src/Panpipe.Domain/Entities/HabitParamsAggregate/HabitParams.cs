using Panpipe.Domain.Interfaces;

namespace Panpipe.Domain.Entities.HabitParamsAggregate;

public class HabitParams<T>: AggregateRoot, IHabitParams where T: IHabitResultType 
{
    #pragma warning disable CS8618 // Required by Entity Framework
    private HabitParams() {}

    public HabitParams
    (
        Guid id, 
        string name, 
        IHabitPeriodicity periodicity, 
        T goal, 
        bool isPublicTemplate
    ): this() 
    {
        Id = id;
        Name = name;
        Periodicity = periodicity;
        Goal = goal;
        IsPublicTemplate = isPublicTemplate;
    }

    public Type ResultType => typeof(T);

    public Guid Id { get; }
    public string Name { get; private set;}
    public IHabitPeriodicity Periodicity { get; private set; }
    public T Goal { get; private set; }
    public bool IsPublicTemplate { get; }

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
