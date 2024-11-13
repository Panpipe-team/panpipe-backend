namespace Panpipe.Domain.Entities;

public class Group: AggregateRoot
{
    private readonly List<Guid> userIds = new List<Guid>();

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public ICollection<Guid> UserIds => userIds.AsReadOnly();

    #pragma warning disable CS8618 // Required by Entity Framework
    private Group() {}

    public Group(Guid id, string name, Guid userId)
    {
        Id = id;
        Name = name;
        
        AddUser(userId);
    }

    public void AddUser(Guid userId) => userIds.Add(userId);
}
