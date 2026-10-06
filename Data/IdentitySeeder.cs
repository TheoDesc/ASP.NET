using Microsoft.AspNetCore.Identity;
using StudentManager.Models.Entities;
using Environment = System.Environment;

namespace StudentManager.Data
{
    public static class IdentitySeeder
    {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in new[] { AdminRole, UserRole })
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            const string adminEmail = "admin@studentmanager.fr";
            string? adminPassword = Environment.GetEnvironmentVariable("adminPassword");

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new System.InvalidOperationException("La variable d'environnement 'adminPassword' n'est pas définie.");
            }

            if (await userManager.FindByEmailAsync(adminEmail) is null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Student Manager",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AdminRole);
                }
            }
        }
    }
}
