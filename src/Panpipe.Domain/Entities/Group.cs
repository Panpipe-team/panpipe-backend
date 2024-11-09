namespace Panpipe.Domain.Entities;

public class Group: AggregateRoot
{
    private readonly List<Guid> userIds = new();

    public Guid Id { get; private set;}
    public string Name { get; private set; }
    public IReadOnlyList<Guid> UserIds => userIds.AsReadOnly();

    private Group() {}

    public Group(Guid id, string name, Guid userId)
    {
        Id = id;
        Name = name;
        
        AddUser(userId);
    }

    public void AddUser(Guid userId) => userIds.Add(userId);
}
