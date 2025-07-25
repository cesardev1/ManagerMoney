using System.Text;
using ManagerMoney.Models;
using ManagerMoney.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ManagerMoney.Controllers;

public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;


    public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
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

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword(string message = "")
    {
        ViewBag.Message = message;
        return View();
    }
    
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
    {
        var message = "Proceso concluido, se enviará un email al correo electrónico para el reset de la contraseña";
        ViewBag.Message = message;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return View();
        }
        
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        var codeBase64 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var link = Url.Action("ResetPassword","User", new { code = codeBase64 }, Request.Scheme);
        await _emailService.SendEmailResetPassword(model.Email, link);
        return View();

    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string code)
    {
        if (code is null)
        {
            var message = "Código no encontrado";
            return RedirectToAction("ForgotPassword", new { message });
        }

        var model = new ResetPasswordVM();
        model.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        return View(model);
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return RedirectToAction("PasswordChanged");
        }
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        return RedirectToAction("PasswordChanged");
    }
    [HttpGet]
    [AllowAnonymous]
    public IActionResult PasswordChanged()
    {
        return View();
    }
}