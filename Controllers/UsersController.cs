using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManager.Data;
using StudentManager.Models.Entities;
using StudentManager.Models.ViewModels;

namespace StudentManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
            var list = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                list.Add(new UserRoleViewModel
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email ?? string.Empty,
                    IsAdmin = await _userManager.IsInRoleAsync(user, IdentitySeeder.AdminRole)
                });
            }

            return View(list);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null) return NotFound();

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "Vous ne pouvez pas modifier votre propre rôle.";
                return RedirectToAction(nameof(Index));
            }

            if (await _userManager.IsInRoleAsync(user, IdentitySeeder.AdminRole))
            {
                await _userManager.RemoveFromRoleAsync(user, IdentitySeeder.AdminRole);
                if (!await _userManager.IsInRoleAsync(user, IdentitySeeder.UserRole))
                {
                    await _userManager.AddToRoleAsync(user, IdentitySeeder.UserRole);
                }
                TempData["Success"] = $"{user.Email} n'est plus administrateur.";
            }
            else
            {
                if (await _userManager.IsInRoleAsync(user, IdentitySeeder.UserRole))
                {
                    await _userManager.RemoveFromRoleAsync(user, IdentitySeeder.UserRole);
                }
                await _userManager.AddToRoleAsync(user, IdentitySeeder.AdminRole);
                TempData["Success"] = $"{user.Email} est maintenant administrateur.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
