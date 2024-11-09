namespace Panpipe.Presentation.Responses;

public class GetGroupResponse(string name, IReadOnlyList<Guid> userIds): BaseResponse
{
    public string Name { get; } = name;
    public IReadOnlyList<Guid> Participants { get; } = userIds;
}
