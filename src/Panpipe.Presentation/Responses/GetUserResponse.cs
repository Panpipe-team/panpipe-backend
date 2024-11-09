namespace Panpipe.Presentation.Responses;

public class GetUserResponse(string login): BaseResponse
{
    public string Login { get; } = login;
}
