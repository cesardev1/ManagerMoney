using ManagerMoney.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ManagerMoney.Controllers;

public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User() { Email = model.Email };
        var result = await _userManager.CreateAsync(user, password: model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: true);
            return RedirectToAction("Index", "Transaction");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Transaction");
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result =
            await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Transaction");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            return View(model);
        }
    }

}