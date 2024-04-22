using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Newsreel.ViewModel;
using MVC.Newsreel.Data;
using MVC.Newsreel.Services;
using Microsoft.AspNetCore.Authorization;
namespace MVC.Newsreel.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;
    public AccountController(UserManager<User> userManager, 
                            SignInManager<User> signInManager,
                            IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User { Email = model.Email, UserName = model.UserName, Name = model.Name};
            // додаємо користувача
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {   
                    await SendEmailConfirmationEmail(user, token);
                }
                await _userManager.AddToRoleAsync(user, "user");
                return RedirectToAction("ConfirmEmail", new { email = model.Email });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        } 
        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                
                // перевіряємо, чи належить URL додатку
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Ви не підтвердили свою адресу пошти!");
            }
            else
            {
                ModelState.AddModelError("", "Неправильний логін чи (та) пароль");
            }
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        // видаляємо автентифікаційні куки
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                ViewBag.IsSuccess = true;
                ModelState.Clear();
                return View();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    private async Task SendEmailConfirmationEmail(User user, string token)
    {
        string appDomain = "http://localhost:5166/";
        string confirmationLink = "confirm-email?uid={0}&token={1}";

        UserEmailOptions options = new UserEmailOptions
        {
            ToEmails = new List<string>() { user.Email },
            PlaceHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{UserName}}", user.Name),
                new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token))
            }
        };

        await _emailService.SendEmailForEmailConfirm(options);
    }

    private async Task SendForgotPasswordEmail(User user, string token)
    {
        string appDomain = "http://localhost:5166/";
        string confirmationLink = "reset-password?uid={0}&token={1}";

        UserEmailOptions options = new UserEmailOptions
        {
            ToEmails = new List<string>() { user.Email },
            PlaceHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{UserName}}", user.Name),
                new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token))
            }
        };

        await _emailService.SendEmailForForgotPassword(options);
    }

    

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
    {
        EmailConfirmModel model = new EmailConfirmModel
        {
            Email = email
        };

        if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
        {
            token = token.Replace(' ', '+');
            var result = await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
            if (result.Succeeded)
            {
                model.EmailVerified = true;
            }
        }

        return View(model);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            if (user.EmailConfirmed)
            {
                model.EmailVerified = true;
                return View(model);
            }

            await _userManager.GenerateEmailConfirmationTokenAsync(user);
            model.EmailSent = true;
            ModelState.Clear();
        }
        else
        {
            ModelState.AddModelError("", "Щось пішло не так або термін дії токену закінчився.");
        }
        return View(model);
    }
    
    [AllowAnonymous] 
    [HttpGet("forgot-password")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (!string.IsNullOrEmpty(token))
                {
                    await SendForgotPasswordEmail(user, token);
                }
            }

            model.EmailSent = true;
        }
        return View(model);
    }

    [AllowAnonymous, HttpGet("reset-password")]
    public IActionResult ResetPassword(string uid, string token)
    {
        ResetPasswordModel resetPasswordModel = new ResetPasswordModel
        {
            Token = token,
            UserId = uid
        };
        return View(resetPasswordModel);
    }

    [AllowAnonymous, HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            model.Token = model.Token.Replace(' ', '+');
            var result = await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                ModelState.Clear();
                model.IsSuccess = true;
                return View(model);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }
}
