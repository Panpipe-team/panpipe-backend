namespace Panpipe.Presentation.Responses;

public class LoginResponse(Guid userId): BaseResponse
{
    public Guid UserId { get; } = userId;
}
