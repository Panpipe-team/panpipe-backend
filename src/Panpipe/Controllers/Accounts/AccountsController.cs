using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Controllers.Auth;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Accounts;

[ApiController]
[Route("/api/v1.1/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager;

    public AccountsController
    (
        UserManager<AppIdentityUser> userManager,
        SignInManager<AppIdentityUser> signInManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPut]
    [Route("name")]
    [TranslateResultToActionResult]
    public async Task<Result> ChangeAccountName([FromBody] ChangeAccountNameRequest request)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        user.FullName = request.NewName;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    [HttpPost]
    [Route("password")]
    [TranslateResultToActionResult]
    public async Task<Result> ChangeAccountPassword([FromBody] ChangeAccountPasswordRequest request)
    {
        const bool lockoutOnFailure = false;

        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var currentPasswordIsCorrect = 
            await _signInManager.CheckPasswordSignInAsync(user, request.PrevPassword, lockoutOnFailure);
        
        if (!currentPasswordIsCorrect.Succeeded)
        {
            return Result.Unauthorized("Wrong password");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.PrevPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return Result.Invalid(result.Errors.Select(error => new ValidationError(error.Code, error.Description)));
        }

        return Result.Success();
    }
}
