using Microsoft.AspNetCore.Mvc;
using NTR.Models;
using Microsoft.AspNetCore.Identity;
using NTR.Data;
using Microsoft.AspNetCore.Authorization;

namespace NTR.Controllers;

[Authorize(Roles = RolesExtensions.DIRECTOR)]
[Route("[controller]")]
[ApiController]
public class ApiPatientsController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public ApiPatientsController(ApplicationDbContext context, UserManager<UserModel> userManager,
                                 SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userManager.GetUsersInRoleAsync(RolesExtensions.PATIENT);
        // sort by .IsAccepted
        var patients = users.OrderBy(p => p.IsAccepted);
        return Ok(patients);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var patient = await _userManager.FindByIdAsync(id);
        if (patient == null)
        {
            return NotFound();
        }

        patient.IsAccepted = true;
        await _context.SaveChangesAsync();

        return Ok();
    }
}
