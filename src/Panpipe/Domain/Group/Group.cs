namespace Panpipe.Domain.Group;

public class Group
{
    private readonly List<Guid> userIds = new ();

    #pragma warning disable CS8618 // Required by Entity Framework
    private Group() { }

    public Group(Guid id, string name, Guid userCreatorId, List<Guid> participantIds)
    {
        Id = id;
        Name = name;

        AddUserId(userCreatorId);
        participantIds.ForEach(participantId => AddUserId(participantId));
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }

    public IList<Guid> UserIds => userIds.AsReadOnly();

    public void AddUserId(Guid userId)
    {
        if (!userIds.Contains(userId))
        {
            userIds.Add(userId);
        }
    }

    public void RemoveUserId(Guid userId)
        => userIds.Remove(userId);
}
