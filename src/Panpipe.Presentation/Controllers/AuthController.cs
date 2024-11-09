using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Persistence.Identity;
using Panpipe.Presentation.Requests;
using Panpipe.Presentation.Responses;

namespace Panpipe.Presentation.Controllers;

[ApiController]
[Route("/api/v1")]
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
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        var username = request.Login;
        var userExists = await _userManager.FindByNameAsync(username);
    
        if (userExists is not null)
        {
            return Result.Conflict($"User with login \"{username}\" already exists").ToActionResult(this);
        }

        var user = new AppIdentityUser(username, null);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return Result.Invalid(result.Errors.Select(error => new ValidationError(error.Code, error.Description)))
                .ToActionResult(this);
        }
        
        return Result.Created(user.Id).Map(id => new RegisterResponse(id)).ToActionResult(this);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var isPersistent = true;
        var lockoutOnFailure = false;
        var userName = request.Login;
        var result = 
            await _signInManager.PasswordSignInAsync(userName, request.Password, isPersistent, lockoutOnFailure);

        if (!result.Succeeded)
        {
            return Result.Unauthorized("Wrong login or password").ToActionResult(this);
        }
        
        var user = await _userManager.FindByNameAsync(userName);

        if (user is null)
        {
            return Result.CriticalError("Signed in user cannot be found").ToActionResult(this);
        }

        return Result.Success(user.Id).Map(guid => new LoginResponse(guid)).ToActionResult(this);
    }

    [HttpPost]
    [Route("logout")]
    public async Task<ActionResult> Login()
    {
        await _signInManager.SignOutAsync();

        return Result.Success().ToActionResult(this);
    }
}