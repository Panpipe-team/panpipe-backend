using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Auth;

[ApiController]
[Route("/api/v1.1")]
public class AuthController: ControllerBase
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager;

    public AuthController
    (
        UserManager<AppIdentityUser> userManager,
        SignInManager<AppIdentityUser> signInManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    [Route("register")]
    [TranslateResultToActionResult]
    public async Task<Result<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        var username = request.Login;
        var userExists = await _userManager.FindByNameAsync(username);

        if (userExists is not null)
        {
            return Result.Conflict($"User with login \"{username}\" already exists");
        }

        var user = new AppIdentityUser(username, request.Name);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Result.Invalid(result.Errors.Select(error => new ValidationError(error.Code, error.Description)));
        }

        return Result.Created(user.Id).Map(id => new RegisterResponse(id));
    }

    [HttpPost]
    [Route("login")]
    [TranslateResultToActionResult]
    public async Task<Result<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var isPersistent = true;
        var lockoutOnFailure = false;
        var userName = request.Login;
        var result = 
            await _signInManager.PasswordSignInAsync(userName, request.Password, isPersistent, lockoutOnFailure);

        if (!result.Succeeded)
        {
            return Result.Unauthorized("Wrong login or password");
        }

        var user = await _userManager.FindByNameAsync(userName);

        if (user is null)
        {
            return Result.CriticalError("Signed in user cannot be found");
        }

        return Result.Success(user.Id).Map(guid => new LoginResponse(guid));
    }

    [HttpPost]
    [Route("logout")]
    [TranslateResultToActionResult]
    public async Task<Result> Logout()
    {
        await _signInManager.SignOutAsync();

        return Result.Success();
    }
}
