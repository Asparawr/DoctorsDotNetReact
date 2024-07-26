using Microsoft.AspNetCore.Mvc;
using doctors.Models;
using Microsoft.AspNetCore.Identity;
using doctors.Data;


namespace doctors.Controllers;

public class LoginController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public LoginController(ApplicationDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TryLogin([Bind("Email,Password")] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return RedirectToAction("UserNotFound");
            }
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }


    public IActionResult UserNotFound()
    {
        return View();
    }
}
