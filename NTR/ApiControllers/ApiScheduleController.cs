using Microsoft.AspNetCore.Mvc;
using NTR.Models;
using Microsoft.AspNetCore.Identity;
using NTR.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace NTR.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class ApiScheduleController : Controller
{
    private readonly UserManager<UserModel> _userManager;
    private readonly SignInManager<UserModel> _signInManager;

    private readonly ApplicationDbContext _context;

    public ApiScheduleController(ApplicationDbContext context, UserManager<UserModel> userManager,
                                 SignInManager<UserModel> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [Authorize(Roles = RolesExtensions.DIRECTOR)]
    [HttpPost("dates")]
    public async Task<IActionResult> Post([FromBody] string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            // Get current user id
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            id = user.Id;
        }
        var schedule = _context.Schedule.Where(s => s.DoctorId == id).ToList();
        // get list of dates
        var scheduleDates = schedule.Select(s => s.Date.Date).Distinct().ToList();
        return Ok(scheduleDates);
    }

    [Authorize(Roles = RolesExtensions.DIRECTOR)]
    [HttpPost("add")]
    public async Task<IActionResult> Post([FromBody] NewScheduleModel model)
    {
        if (ModelState.IsValid)
        {
            model.Date = model.Date.Date.Add(model.WorkDayStart);
            for (DateTime date = model.Date; date.TimeOfDay < model.WorkDayEnd; date = date.AddMinutes(15))
            {
                // Check if already exists
                var schedule =
                    await _context.Schedule.FirstOrDefaultAsync(s => s.DoctorId == model.DoctorId && s.Date == date);
                if (schedule != null)
                {
                    continue;
                }
                schedule = new() { DoctorId = model.DoctorId, Date = date };
                _context.Schedule.Add(schedule);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
        return BadRequest();
    }

    [Authorize(Roles = RolesExtensions.DIRECTOR)]
    [HttpPost("remove")]
    public async Task<IActionResult> Post([FromBody] ScheduleDateModel model)
    {
        if (ModelState.IsValid)
        {
            model.Date = model.Date.Date;
            var schedule = await _context.Schedule.Where(s => s.DoctorId == model.DoctorId && s.Date.Date == model.Date).ToListAsync();
            _context.Schedule.RemoveRange(schedule);
            await _context.SaveChangesAsync();
            return Ok();
        }
        return BadRequest();
    }

    [Authorize(Roles = RolesExtensions.DIRECTOR)]
    [HttpPost("copy_week")]
    public async Task<IActionResult> PostWeek([FromBody] ScheduleDateModel model)
    {
        if (ModelState.IsValid)
        {
            model.Date = model.Date.Date;
            var prevWeek = model.Date.AddDays(-7);
            for (DateTime date = prevWeek; date < model.Date.Date; date = date.AddDays(1))
            {
                var newDate = date.AddDays(7);

                // Clear schedule for this date
                var scheduleToRemove = await _context.Schedule.Where(s => s.DoctorId == model.DoctorId && s.Date.Date == newDate).ToListAsync();
                _context.Schedule.RemoveRange(scheduleToRemove);

                // Copy schedule for this date
                var schedule = await _context.Schedule.Where(s => s.DoctorId == model.DoctorId && s.Date.Date == date).ToListAsync();
                foreach (var scheduleItem in schedule.Where(s => s.Date.Date == date))
                {
                    var newScheduleItem = new ScheduleModel() { DoctorId = model.DoctorId, Date = newDate.Add(scheduleItem.Date.TimeOfDay) };
                    _context.Schedule.Add(newScheduleItem);
                }
            }
            await _context.SaveChangesAsync();
            return Ok();
        }
        return BadRequest();
    }

    [Authorize(Roles = RolesExtensions.DIRECTOR)]
    [HttpPost("select")]
    public async Task<IActionResult> PostSelect([FromBody] ScheduleDateModel model)
    {
        if (ModelState.IsValid) 
        {
            model.Date = model.Date.Date;
            var schedule = await _context.Schedule.Where(s => s.DoctorId == model.DoctorId && s.Date.Date == model.Date).ToListAsync();
            if (schedule.Count == 0)
            {
                return NotFound();
            }
            var WorkDayStart = schedule.Min(s => s.Date.TimeOfDay);
            var WorkDayEnd = schedule.Max(s => s.Date.TimeOfDay) + TimeSpan.FromMinutes(15);
            return Ok(new { WorkDayStart, WorkDayEnd });
        }
        return BadRequest();
    }

    // Return schedules with specialization
    [Authorize(Roles = RolesExtensions.PATIENT)]
    [HttpPost("specialization")]
    public async Task<IActionResult> PostSpecialization([FromBody] string specialization)
    {
        if (ModelState.IsValid)
        {
            var schedules = await _context.Schedule.Join(_context.Users, s => s.DoctorId, u => u.Id, (s, u) => new SchedulePatientViewModel
            {
                Id = s.Id,
                DoctorId = s.DoctorId,
                PatientId = s.PatientId,
                Date = s.Date,
                Description = s.Description,
                DoctorName = u.Name + " " + u.Surname,
                Specialization = u.Specialization
            }).Where(s => s.Specialization == specialization && s.PatientId == null && DateTime.Compare(s.Date, DateTime.Now) > 0
            ).OrderBy(s => s.Date).ToListAsync();
            
            return Ok(schedules);
        }

        return BadRequest();
    }

    // Plan for current user
    [Authorize(Roles = RolesExtensions.PATIENT)]
    [HttpPost("plan")]
    public async Task<IActionResult> PostPlan ([FromBody] int scheduleId)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var schedule = await _context.Schedule.Where(s => s.Id == scheduleId).FirstOrDefaultAsync();
            if (schedule == null)
            {
                return NotFound();
            }
            schedule.PatientId = user.Id;
            await _context.SaveChangesAsync();
            return Ok();
        }

        return BadRequest();
    }

    // Get planned for current user
    [Authorize(Roles = RolesExtensions.PATIENT)]
    [HttpGet("get_planned")]
    public async Task<IActionResult> GetPlanned()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var schedules = await _context.Schedule.Join(_context.Users, s => s.DoctorId, u => u.Id, (s, u) => new SchedulePatientViewModel
            {
                Id = s.Id,
                DoctorId = s.DoctorId,
                PatientId = s.PatientId,
                Date = s.Date,
                Description = s.Description,
                DoctorName = u.Name + " " + u.Surname,
                Specialization = u.Specialization
            }).Where(s => s.PatientId == user.Id && DateTime.Compare(s.Date, DateTime.Now) > 0 && s.Description == null
            ).OrderBy(s => s.Date).ToListAsync();
            
            return Ok(schedules);
        }

        return BadRequest();
    }

    // Get history for current user, with descriptions
    [Authorize(Roles = RolesExtensions.PATIENT)]
    [HttpGet("get_history")]
    public async Task<IActionResult> GetHistory()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var schedules = await _context.Schedule.Join(_context.Users, s => s.DoctorId, u => u.Id, (s, u) => new SchedulePatientViewModel
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
            
            return Ok(schedules);
        }

        return BadRequest();
    }

    // Cancel for current user
    [Authorize(Roles = RolesExtensions.PATIENT)]
    [HttpPost("cancel")]
    public async Task<IActionResult> PostCancel ([FromBody] int scheduleId)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var schedule = await _context.Schedule.Where(s => s.Id == scheduleId).FirstOrDefaultAsync();
            if (schedule == null)
            {
                return NotFound();
            }
            schedule.PatientId = null;
            await _context.SaveChangesAsync();
            return Ok();
        }

        return BadRequest();
    }

    // Get all visits for current doctor
    [Authorize(Roles = RolesExtensions.DOCTOR)]
    [HttpGet("get_doctor_visits")]
    public async Task<IActionResult> GetDoctor()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var schedules = await _context.Schedule.Join(_context.Users, s => s.PatientId, u => u.Id, (s, u) => new ScheduleDoctorViewModel
            {
                Id = s.Id,
                DoctorId = s.DoctorId,
                PatientId = s.PatientId,
                Date = s.Date,
                Description = s.Description,
                PatientName = u.Name + " " + u.Surname
            }).Where(s => s.DoctorId == user.Id && DateTime.Compare(s.Date, DateTime.Now) > 0 && s.Description == null
            ).OrderBy(s => s.Date).ToListAsync();
            
            return Ok(schedules);
        }

        return BadRequest();
    }

    // Get all scheduled for current doctor
    [Authorize(Roles = RolesExtensions.DOCTOR)]
    [HttpGet("get_doctor_scheduled")]
    public async Task<IActionResult> GetDoctorScheduled()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var schedules = await _context.Schedule.Where(s => s.DoctorId == user.Id 
                && DateTime.Compare(s.Date, DateTime.Now) > 0
                ).OrderBy(s => s.Date).ToListAsync();

            if (schedules.Count == 0)
            {
                return Ok();
            }

            //make list of ScheduleDayModel for each day StartTime is min, EndTime is max
            var scheduleDays = new List<ScheduleDayModel>();
            ScheduleDayModel currentScheduleDay = new() { 
                Date = schedules[0].Date.Date, 
                StartTime = schedules[0].Date.TimeOfDay, 
                EndTime = schedules[0].Date.TimeOfDay + TimeSpan.FromMinutes(15)
                };
            foreach (var schedule in schedules)
            {
                if (currentScheduleDay.Date != schedule.Date.Date)
                {
                    scheduleDays.Add(currentScheduleDay);
                    currentScheduleDay = new ScheduleDayModel() { 
                        Date = schedule.Date.Date, 
                        StartTime = schedule.Date.TimeOfDay, 
                        EndTime = schedule.Date.TimeOfDay + TimeSpan.FromMinutes(15)
                        };
                } else {
                    currentScheduleDay.EndTime = schedule.Date.TimeOfDay + TimeSpan.FromMinutes(15);
                }
            }
            scheduleDays.Add(currentScheduleDay);


            return Ok(scheduleDays);
        }

        return BadRequest();
    }

    // Add description for current doctor
    [Authorize(Roles = RolesExtensions.DOCTOR)]
    [HttpPost("add_desc")]
    public async Task<IActionResult> PostDescription ([FromBody] ScheduleDescriptionModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var schedule = await _context.Schedule.Where(s => s.Id == model.Id).FirstOrDefaultAsync();
            if (schedule == null)
            {
                return NotFound();
            }
            schedule.Description = model.Description;
            await _context.SaveChangesAsync();
            return Ok();
        }

        return BadRequest();
    }
}
