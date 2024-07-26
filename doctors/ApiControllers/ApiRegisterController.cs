using Microsoft.AspNetCore.Mvc;
using doctors.Models;
using Microsoft.AspNetCore.Identity;
using doctors.Data;
using System.Diagnostics;

namespace doctors.Controllers;

[Route("[controller]")]
[ApiController]
public class ApiRegisterController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public ApiRegisterController(ApplicationDbContext context, UserManager<UserModel> userManager,
                                 SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Post([Bind("Name,Surname,Email,Password,ConfirmPassword")] RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            Debug.WriteLine("Model is valid");
            Debug.WriteLine(model);
            var user = new UserModel { Name = model.Name, Surname = model.Surname, Email = model.Email,
                                       UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,
                                                  RolesExtensions.PATIENT); // Add "Patient" role to the registered user
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok("Patient");
            }
            else
            {
                return NotFound();
            }
        }
        return BadRequest();
    }
}
