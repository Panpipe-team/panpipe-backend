namespace Panpipe.Presentation.Responses;

public class RegisterResponse(Guid userId): BaseResponse
{
    public Guid UserId { get; } = userId;
}
