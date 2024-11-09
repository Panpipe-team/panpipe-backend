namespace Panpipe.Presentation.Responses;

public class CreateGroupResponse(Guid id): BaseResponse
{
    public Guid GroupId { get; } = id;
}
