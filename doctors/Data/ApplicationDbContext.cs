using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using doctors.Models;

namespace doctors.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<doctors.Models.ScheduleModel> Schedule { get; set; } = default!;

        public static async void AddDefaultAccount(IServiceScope scope)
        {
            // Check if default user exists
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            var user = await userManager.FindByEmailAsync("dir@dir");
            if (user != null)
            {
                return;
            }
            // Add default user
            user = new UserModel { Email = "dir@dir", UserName = "dir@dir" };
            var result = await userManager.CreateAsync(user, "Dir11!!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, RolesExtensions.DIRECTOR);
            }
        }
    }
}
