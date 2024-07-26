using Microsoft.AspNetCore.Mvc;
using doctors.Models;
using Microsoft.AspNetCore.Identity;
using doctors.Data;

namespace doctors.Controllers;

[Route("[controller]")]
[ApiController]
public class ApiLoginController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public ApiLoginController(ApplicationDbContext context, UserManager<UserModel> userManager,
                              SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                                                                  lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound();
                }
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null)
                {
                    return NotFound();
                }
                var role = roles[0];
                if (role == RolesExtensions.PATIENT)
                {
                    var isAccepted = user.IsAccepted;
                    if (isAccepted == false)
                    {
                        return Ok();
                    }
                }
                return Ok(role);
            }
            else
            {
                return BadRequest();
            }
        }
        return BadRequest();
    }
}
