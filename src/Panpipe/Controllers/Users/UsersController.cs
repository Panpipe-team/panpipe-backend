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
    public async Task<Result<GetUserResponse>> Get([FromQuery] string? login)
    {
        if (login is null)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                return Result.Unauthorized("Cannot find authorized user by claim");
            }

            if (user.UserName is null)
            {
                return Result.CriticalError($"Current user does not have login");
            }

            return Result.Success(new GetUserResponse(user.Id, user.UserName, user.FullName));
        }

        var result = await _userManager.FindByNameAsync(login);

        if (result is null)
        {
            return Result.NotFound();
        }

        return Result.Success(new GetUserResponse(result.Id, login, result.FullName));
    }
}