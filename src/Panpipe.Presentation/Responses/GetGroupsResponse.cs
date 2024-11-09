namespace Panpipe.Presentation.Responses;

public class GetGroupsResponseGroup(Guid id, string name)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set;} = name;
}
