using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using doctors.Data;
using doctors.Models;

namespace doctors.Controllers
{
    [Authorize(Roles = RolesExtensions.DOCTOR)]
    public class ScheduleController : Controller
    {
        private readonly UserManager<UserModel> _userManager;

        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }
            // Get all doctor's schedule with matching id
            var schedule = await _context.Schedule.Where(s => s.DoctorId == user.Id).ToListAsync();
            // split to list of lists by date
            var scheduleByDate = schedule.GroupBy(s => s.Date.Date).Select(g => g.ToList()).ToList();
            return View(scheduleByDate);
        }
    }
}
