using Microsoft.AspNetCore.Mvc;
using NTR.Models;
using Microsoft.AspNetCore.Identity;
using NTR.Data;
using Microsoft.AspNetCore.Authorization;

namespace NTR.Controllers;

[Authorize(Roles = RolesExtensions.DIRECTOR)]
[Route("[controller]")]
[ApiController]
public class ApiReportController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public ApiReportController(ApplicationDbContext context, UserManager<UserModel> userManager,
                                SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ReportRequestModel model) 
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        // for each doctor count all Schedules in given time range
        var doctors = await _userManager.GetUsersInRoleAsync(RolesExtensions.DOCTOR);
        List<ReportModel> reports = new();
        foreach (var doctor in doctors)
        {
            var workHours = _context.Schedule.Where(s => s.DoctorId == doctor.Id && s.Date >= model.StartDate && s.Date <= model.EndDate).Count()*15/60;
            var visitCount = _context.Schedule.Where(s => s.DoctorId == doctor.Id && s.Date >= model.StartDate && s.Date <= model.EndDate && s.Description != null).Count();
            reports.Add(new ReportModel
            {
                DoctorName = doctor.Name + " " + doctor.Surname,
                WorkHours = workHours,
                VisitCount = visitCount
            });
        }

        return Ok(reports);
    }
}
