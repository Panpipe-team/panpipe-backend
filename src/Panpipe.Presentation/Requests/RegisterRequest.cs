namespace Panpipe.Presentation.Requests;

public class RegisterRequest(string login, string password): BaseRequest
{
    public string Login { get; } = login;
    public string Password { get; } = password;
}
