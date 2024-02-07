using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NTR.Data;
using NTR.Models;

namespace NTR.Controllers
{
    [Authorize(Roles = RolesExtensions.DIRECTOR)]
    public class PatientController : Controller
    {
        private readonly UserManager<UserModel> _userManager;

        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync(RolesExtensions.PATIENT);
            // sort by .IsAccepted
            var patients = users.OrderBy(p => p.IsAccepted);
            return View(patients);
        }

        public async Task<IActionResult> Details(string? id)
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

            return View(patient);
        }

        public async Task<IActionResult> Accept(string? id)
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

            return RedirectToAction(nameof(Index));
        }
    }
}
