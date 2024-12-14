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

        return Result.Success(new GetUserByIdResponse(userName, user.FullName));
    }

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserByLoginResponse>> GetByLogin([FromQuery] string login)
    {
        var result = await _userManager.FindByNameAsync(login);

        if (result is null)
        {
            return Result.NotFound();
        }

        return Result.Success(new GetUserByLoginResponse(result.Id, result.FullName));
    }
}