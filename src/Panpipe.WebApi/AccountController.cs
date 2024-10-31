using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Panpipe.WebApi.ViewModels;

namespace Panpipe.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        // _signInManager = signInManager;
    }
    [HttpGet]
    public IActionResult Register() => View();
    
    [HttpPost]
     public async Task<IActionResult> Register(RegisterViewModel model)//string email, string password)
     {
         
         var user = new IdentityUser {UserName = model.Email, Email = model.Email};
         var result = await _userManager.CreateAsync(user, model.Password);
         if (result.Succeeded)
         {
             await _signInManager.SignInAsync(user, false);
             return RedirectToAction("Index", "Home");
         }

         foreach (var error in result.Errors)
         {
             ModelState.AddModelError(string.Empty, error.Description);
         }
         
         return View(model);
     }
     
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
     
}

