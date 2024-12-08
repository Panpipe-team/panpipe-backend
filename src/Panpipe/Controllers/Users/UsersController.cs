using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Users;

[ApiController]
[Route("/api/v1.1/[controller]")]
[Authorize]
public class UsersController(UserManager<AppIdentityUser> userManager): ControllerBase
{
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [Route("{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserByIdResponse>> GetById([FromRoute] Guid id)
    {
        const string ReplacementForNullName = "";

        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return Result.NotFound();
        }

        if (user.UserName is null)
        {
            return Result.CriticalError($"User with id {id} does not have login");
        }

        var userName = user.UserName;

        return Result.Success(user).Map(user => new GetUserByIdResponse(userName, user.FullName ?? ReplacementForNullName));
    }

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserByLoginResponse>> GetByLogin([FromQuery] string Login)
    {
        // FAKED
        return Result.Success().Map(_ => new GetUserByLoginResponse(Guid.Empty, "Faked user name"));
    }
}