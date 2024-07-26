using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using doctors.Data;
using doctors.Models;

namespace doctors.Controllers
{
    [Authorize(Roles = RolesExtensions.DIRECTOR)]
    public class DoctorController : Controller
    {
        private readonly UserManager<UserModel> _userManager;

        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUsersInRoleAsync(RolesExtensions.DOCTOR));
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _userManager.FindByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        public async Task<IActionResult> Accept(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _userManager.FindByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            doctor.IsAccepted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Specialization,Name,Surname,Email,Password,ConfirmPassword")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel { Name = model.Name, Surname = model.Surname, Email = model.Email,
                                           UserName = model.Email, Specialization = model.Specialization };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RolesExtensions.DOCTOR); // Add "Doctor" role to the registered user
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("UserAlreadyExists");
                }
            }
            return RedirectToAction("UserAlreadyExists");
        }

        public IActionResult UserAlreadyExists()
        {
            return View();
        }

        public IActionResult Schedule(string? id)
        {
            // Get all doctor's schedule with matching id
            var schedule = _context.Schedule.Where(s => s.DoctorId == id).OrderBy(s => s.Date).ToList();
            // split to list of lists by date
            var scheduleByDate = schedule.GroupBy(s => s.Date.Date).Select(g => g.ToList()).ToList();
            return View((id, scheduleByDate));
        }

        public async Task<IActionResult> TrySchedule([Bind("DoctorId,Date,WorkDayStart,WorkDayEnd")] NewScheduleModel model)
        {
            model.Date = model.Date.Date.Add(model.WorkDayStart);
            for (DateTime date = model.Date; date.TimeOfDay < model.WorkDayEnd; date = date.AddMinutes(15))
            {
                // Check if already exists
                var schedule = await _context.Schedule.FirstOrDefaultAsync(s => s.DoctorId == model.DoctorId && s.Date == date);
                if (schedule != null)
                {
                    continue;
                }
                schedule = new() {DoctorId = model.DoctorId, Date = date};
                _context.Schedule.Add(schedule);
            }

            if (ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Schedule", new { id = model.DoctorId });
            }
            return RedirectToAction("ScheduleInvalid");
        }

        public IActionResult ScheduleInvalid()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CopyLastWeek(string? doctorId)
        {
            if (doctorId == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedule.Where(s => s.DoctorId == doctorId).OrderBy(s => s.Date).ToListAsync();
            var lastWeek = schedule.Where(s => s.Date.Date >= schedule[schedule.Count - 1].Date.Date.AddDays(-7)).ToList();
            var lastWeekByDate = lastWeek.GroupBy(s => s.Date.Date).Select(g => g.ToList()).ToList();

            foreach (var day in lastWeekByDate)
            {
                foreach (var scheduleModel in day)
                {
                    var newSchedule = new ScheduleModel() { DoctorId = doctorId, Date = scheduleModel.Date.AddDays(7) };
                    _context.Schedule.Add(newSchedule);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Schedule", new { id = doctorId });
        }

        public async Task<IActionResult> DeleteSchedule(string? doctorId, DateTime? date)
        {
            if (doctorId == null || date == null)
            {
                return NotFound();
            }

            var schedules = await _context.Schedule.Where(s => s.DoctorId == doctorId && s.Date.Date == date.Value.Date).ToListAsync();
            if (schedules == null)
            {
                return NotFound();
            }

            foreach (var schedule in schedules)
            {
                _context.Schedule.Remove(schedule);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Schedule", new { id = doctorId });
        }

    }
}
