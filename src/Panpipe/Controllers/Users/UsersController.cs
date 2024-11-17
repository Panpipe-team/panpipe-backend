using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Users;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize]
public class UsersController: ControllerBase
{
    private readonly UserManager<AppIdentityUser> _userManager;

    public UsersController(UserManager<AppIdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserResponse>> Get([FromRoute] Guid id)
    {
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

        return Result.Success(user).Map(user => new GetUserResponse(userName));
    }
}