using System.Collections;

namespace Panpipe.Domain.Group;

public class Group
{
    private readonly List<Guid> userIds = new ();

    #pragma warning disable CS8618 // Required by Entity Framework
    private Group() { }

    public Group(Guid id, string name, Guid userCreatorId)
    {
        Id = id;
        Name = name;
        userIds.Add(userCreatorId);
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }

    public IList<Guid> UserIds => userIds.AsReadOnly();
}
