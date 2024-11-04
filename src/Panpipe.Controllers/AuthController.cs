using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Domain.Entities;

namespace Panpipe.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;

    public AuthController(UserManager<Account> userManager, SignInManager<Account> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public record RegisterParameters(string email, string password);

    [HttpPost]
    public async Task<ActionResult> Register(RegisterParameters registerParameters)
    {
        var account = new Account()
        {
            Email = registerParameters.email,
            UserName = registerParameters.email,
        };

        var creationResult = await _userManager.CreateAsync(account, registerParameters.password);

        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult.Errors.Select(err=>err.Code + " : " + err.Description));
        }
        return Ok();
    }

    public record LoginParameters(string email, string password, bool isPersistent);

    [HttpPost]
    public async Task<ActionResult> Login(LoginParameters loginParameters)
    {
        var account = await _userManager.FindByEmailAsync(loginParameters.email);

        if (account == null)
            return BadRequest("email не найден");

        var passwordCheckResult = await _userManager.CheckPasswordAsync(account, loginParameters.password);

        if (passwordCheckResult == false)
            return BadRequest("неверный пароль");

        await _signInManager.SignInAsync(account, loginParameters.isPersistent);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<string> GetMyEmail()
    {
        var user = this.User;
        var email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (email == null)
            throw new InvalidOperationException("что-то пошло не так");
        return email;
    }

    [Authorize]
    [HttpGet]
    public async Task<string> GetMyEmail1()
    {
        var account = await _userManager.GetUserAsync(this.User);

        if (account?.Email == null)
            throw new InvalidOperationException("что-то пошло не так");

        return account.Email;
    }
}