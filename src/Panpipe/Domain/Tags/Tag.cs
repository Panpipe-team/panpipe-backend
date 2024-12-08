namespace Panpipe.Domain.Tags;

public class Tag(Guid id, string name)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
}
