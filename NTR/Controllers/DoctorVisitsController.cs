using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTR.Data;
using NTR.Models;

namespace NTR.Controllers;

[Authorize(Roles = RolesExtensions.DOCTOR)]
public class DoctorVisitsController : Controller
{
    private readonly UserManager<UserModel> _userManager;

    private readonly ApplicationDbContext _context;

    public DoctorVisitsController(ApplicationDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // Get all visits for current user
        var visits = await _context.Schedule.Join(_context.Users, s => s.PatientId, u => u.Id, (s, u) => new ScheduleDoctorViewModel
        {
            Id = s.Id,
            DoctorId = s.DoctorId,
            PatientId = s.PatientId,
            Date = s.Date,
            Description = s.Description,
            PatientName = u.Name + " " + u.Surname,
        }).Where(s => s.DoctorId == _userManager.GetUserId(User) && s.Description == null && DateTime.Compare(s.Date, DateTime.Now) > 0
        ).OrderBy(s => s.Date).ToListAsync();

        return View(visits);
    }

    public async Task<IActionResult> AddDescription(int id)
    {
        var schedule = await _context.Schedule.Where(s => s.Id == id).Join(_context.Users, s => s.PatientId, u => u.Id, (s, u) => new ScheduleDoctorViewModel
        {
            Id = s.Id,
            DoctorId = s.DoctorId,
            PatientId = s.PatientId,
            Date = s.Date,
            Description = s.Description,
            PatientName = u.Name + " " + u.Surname,
        }).FirstOrDefaultAsync();
        if (schedule == null)
        {
            return RedirectToAction("Index");
        }

        return View(schedule);
    }

    [HttpPost]
    public async Task<IActionResult> SetDescription(ScheduleDoctorViewModel schedule)
    {
        var scheduleToUpdate = await _context.Schedule.Where(s => s.Id == schedule.Id).FirstOrDefaultAsync();
        if (scheduleToUpdate == null)
        {
            return RedirectToAction("Index");
        }

        scheduleToUpdate.Description = schedule.Description;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
