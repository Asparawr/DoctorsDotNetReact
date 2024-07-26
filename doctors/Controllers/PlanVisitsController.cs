using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using doctors.Data;
using doctors.Models;

namespace doctors.Controllers;

[Authorize(Roles = RolesExtensions.PATIENT)]
public class PlanVisitsController : Controller
{
    private readonly UserManager<UserModel> _userManager;

    private readonly ApplicationDbContext _context;

    public PlanVisitsController(ApplicationDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SelectSpecialization(SpecializationModel model)
    {
        // Join schedules with doctors
        var schedules = await _context.Schedule.Join(_context.Users, s => s.DoctorId, u => u.Id, (s, u) => new SchedulePatientViewModel
        {
            Id = s.Id,
            DoctorId = s.DoctorId,
            PatientId = s.PatientId,
            Date = s.Date,
            Description = s.Description,
            DoctorName = u.Name + " " + u.Surname,
            Specialization = u.Specialization
        }).Where(s => s.Specialization == model.Specialization && s.PatientId == null && DateTime.Compare(s.Date, DateTime.Now) > 0
        ).OrderBy(s => s.Date).ToListAsync();

        return View((model.Specialization, schedules));
    }

    public async Task<IActionResult> ChooseSchedule(int scheduleId)
    {
        var schedules = await _context.Schedule.Where(s => s.Id == scheduleId && s.PatientId == null).FirstOrDefaultAsync();
        if (schedules == null)
        {
            return RedirectToAction("Index");
        }

        schedules.PatientId = _userManager.GetUserId(User);
        await _context.SaveChangesAsync();

        return RedirectToAction("Scheduled");
    }

    public IActionResult Scheduled()
    {
        return View();
    }
}
