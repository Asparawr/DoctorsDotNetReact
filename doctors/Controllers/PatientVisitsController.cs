using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using doctors.Data;
using doctors.Models;

namespace doctors.Controllers;

[Authorize(Roles = RolesExtensions.PATIENT)]
public class PatientVisitsController : Controller
{
    private readonly UserManager<UserModel> _userManager;

    private readonly ApplicationDbContext _context;

    public PatientVisitsController(ApplicationDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
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
        }).Where(s => s.PatientId == _userManager.GetUserId(User) && (s.Description == null || DateTime.Compare(s.Date, DateTime.Now) < 0)
        ).OrderBy(s => s.Date).ToListAsync();

        return View(visits);
    }

    [HttpPost]
    public async Task<IActionResult> CancelVisit(int id)
    {
        var user = _userManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return NotFound();
        }
        
        var schedule = await _context.Schedule.Where(s => s.Id == id && s.Description == null && s.PatientId == user.Id).FirstOrDefaultAsync();
        if (schedule == null)
        {
            return RedirectToAction("Index");
        }

        schedule.PatientId = null;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
