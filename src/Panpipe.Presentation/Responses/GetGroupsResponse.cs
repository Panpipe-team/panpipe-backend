namespace Panpipe.Presentation.Responses;

public class GetGroupsResponse(List<GetGroupsResponseGroup> groups): BaseResponse
{
    public List<GetGroupsResponseGroup> Groups { get; } = groups;
}

public class GetGroupsResponseGroup(Guid id, string name)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}
