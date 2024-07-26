using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using doctors.Data;
using doctors.Models;

namespace doctors.Controllers;

[Authorize(Roles = RolesExtensions.DIRECTOR)]
public class ReportController : Controller
{
    private readonly UserManager<UserModel> _userManager;

    private readonly ApplicationDbContext _context;

    public ReportController(ApplicationDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ShowReport(DateTime dateFrom, DateTime dateTo)
    {
        // for each doctor count all Schedules in given time range
        var doctors = await _userManager.GetUsersInRoleAsync(RolesExtensions.DOCTOR);
        List<ReportModel> reports = new List<ReportModel>();
        foreach (var doctor in doctors)
        {
            var workHours = _context.Schedule.Where(s => s.DoctorId == doctor.Id && s.Date >= dateFrom && s.Date <= dateTo).Count()*15/60;
            var visitCount = _context.Schedule.Where(s => s.DoctorId == doctor.Id && s.Date >= dateFrom && s.Date <= dateTo && s.Description != null).Count();
            reports.Add(new ReportModel
            {
                DoctorName = doctor.Name + " " + doctor.Surname,
                WorkHours = workHours,
                VisitCount = visitCount
            });
        }
        return View(reports);
    }
}
