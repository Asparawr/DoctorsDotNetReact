using Microsoft.AspNetCore.Mvc;
using doctors.Models;
using Microsoft.AspNetCore.Identity;
using doctors.Data;
using Microsoft.AspNetCore.Authorization;


namespace doctors.Controllers;

public class RegisterController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public RegisterController(ApplicationDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TryRegister([Bind("Name,Surname,Email,Password,ConfirmPassword")] RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new UserModel { Name = model.Name, Surname = model.Surname, Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RolesExtensions.PATIENT); // Add "Patient" role to the registered user
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("UserAlreadyExists");
            }
        }
        return RedirectToAction("UserAlreadyExists");
    }

    
    public IActionResult UserAlreadyExists()
    {
        return View();
    }
}
