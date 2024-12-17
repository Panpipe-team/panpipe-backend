namespace Panpipe.Controllers.Users;

public record GetUserResponse(Guid UserId, string Login, string Name);
