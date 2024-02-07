using Microsoft.AspNetCore.Mvc;
using NTR.Models;
using Microsoft.AspNetCore.Identity;
using NTR.Data;
using Microsoft.AspNetCore.Authorization;

namespace NTR.Controllers;

[Authorize(Roles = RolesExtensions.DIRECTOR)]
[Route("[controller]")]
[ApiController]
public class ApiDoctorsController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public ApiDoctorsController(ApplicationDbContext context, UserManager<UserModel> userManager,
                                SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userManager.GetUsersInRoleAsync(RolesExtensions.DOCTOR);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new UserModel { Name = model.Name, Surname = model.Surname, Email = model.Email,
                                       UserName = model.Email, Specialization = model.Specialization };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RolesExtensions.DOCTOR);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
        return BadRequest();
    }
}
