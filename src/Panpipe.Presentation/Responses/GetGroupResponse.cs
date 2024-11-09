namespace Panpipe.Presentation.Responses;

public class GetGroupResponse(string name, List<Guid> userIds): BaseResponse
{
    public string Name { get; } = name;
    public List<Guid> Participants { get; } = userIds;
}
