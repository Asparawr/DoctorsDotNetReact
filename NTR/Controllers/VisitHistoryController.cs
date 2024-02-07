using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTR.Data;
using NTR.Models;

namespace NTR.Controllers;

[Authorize(Roles = RolesExtensions.PATIENT)]
public class VisitHistoryController : Controller
{
    private readonly UserManager<UserModel> _userManager;

    private readonly ApplicationDbContext _context;

    public VisitHistoryController(ApplicationDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return NotFound();
        }

        // Get all visits for current user
        var visits = await _context.Schedule.Join(_context.Users, s => s.DoctorId, u => u.Id, (s, u) => new SchedulePatientViewModel
        {
            Id = s.Id,
            DoctorId = s.DoctorId,
            PatientId = s.PatientId,
            Date = s.Date,
            Description = s.Description,
            DoctorName = u.Name + " " + u.Surname,
            Specialization = u.Specialization
        }).Where(s => s.PatientId == user.Id && s.Description != null
        ).OrderBy(s => s.Date).ToListAsync();

        return View(visits);
    }

    public async Task<IActionResult> VisitDetails(int id)
    {
        // Get visit details
        var visit = await _context.Schedule.Join(_context.Users, s => s.DoctorId, u => u.Id, (s, u) => new SchedulePatientViewModel
        {
            Id = s.Id,
            DoctorId = s.DoctorId,
            PatientId = s.PatientId,
            Date = s.Date,
            Description = s.Description,
            DoctorName = u.Name + " " + u.Surname,
            Specialization = u.Specialization
        }).FirstOrDefaultAsync(s => s.Id == id);

        return View(visit);
    }
}
