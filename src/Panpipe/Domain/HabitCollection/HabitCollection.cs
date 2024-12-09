namespace Panpipe.Domain.HabitCollection;

public class HabitCollection
{
    private readonly List<Guid> _habitIds;

    #pragma warning disable CS8618 // Required by Entity Framework
    private HabitCollection() {}
    public HabitCollection(Guid id, Guid paramsSetId, Guid groupId, List<Guid> habitIds)
    {
        Id = id;
        ParamsSetId = paramsSetId;
        GroupId = groupId;
        _habitIds = habitIds;
    }

    public Guid Id { get; init; }
    public Guid ParamsSetId { get; init; }
    public Guid GroupId { get; init; }

    public IList<Guid> HabitIds => _habitIds.AsReadOnly();
}
