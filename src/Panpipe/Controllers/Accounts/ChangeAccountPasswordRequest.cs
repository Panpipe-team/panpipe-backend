namespace Panpipe.Controllers.Auth;

public record ChangeAccountPasswordRequest(string PrevPassword, string NewPassword);
