namespace Panpipe.Presentation.Requests;

public class LoginRequest(string login, string password): BaseRequest
{
    public string Login { get; } = login;
    public string Password { get; } = password;
}
