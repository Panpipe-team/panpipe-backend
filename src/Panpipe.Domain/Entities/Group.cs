namespace Panpipe.Domain.Entities;

public class Group: AggregateRoot
{
    private readonly List<Guid> userIds = new();
    public string Name { get; private set; }
    public IReadOnlyList<Guid> UserIds => userIds.AsReadOnly();

    public Group(string name, Guid userId)
    {
        Name = name;
        userIds.Add(userId);
    }
}
