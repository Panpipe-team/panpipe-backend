using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Persistence.Identity;
using Panpipe.Presentation.Responses;

namespace Panpipe.Presentation.Controllers;

[ApiController]
[Route("/api/v1/users")]
public class UserController: ControllerBase
{
    private readonly UserManager<AppIdentityUser> _userManager;

    public UserController(UserManager<AppIdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<GetUserResponse>> Get([FromRoute] Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return Result.NotFound().ToActionResult(this);
        }

        if (user.UserName is null)
        {
            return Result.CriticalError($"User with id {id} does not have login").ToActionResult(this);
        }

        var userName = user.UserName;

        return Result.Success(user).Map(user => new GetUserResponse(userName)).ToActionResult(this);
    }
}