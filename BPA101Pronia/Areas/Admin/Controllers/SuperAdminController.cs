using BPA101Pronia.Areas.Admin.ViewModels.Account.Admin;
using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPA101Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SuperAdminController(AppDbContext db, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> GetUsers()
        {
            List<AppUser> users = await _db.Users
                .Where(u => !u.IsAdmin)
                .ToListAsync();
            return View(users);
        }
        public async Task<IActionResult> GetAdmins()
        {
            List<AppUser> admins = await _db.Users
                .Where(u => u.IsAdmin)
                .ToListAsync();
            return View(admins);
        }


        public IActionResult CreateAdmin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminVM adminVM)
        {
            if (!ModelState.IsValid) return View(adminVM);

            AppUser admin = new AppUser
            {
                Name = "Admin",
                Surname = "Admin",
                UserName = adminVM.Username,
                Email = adminVM.Email,
                IsAdmin = true
            };

            await _userManager.CreateAsync(admin, adminVM.Password);

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(admin, "Admin");

            return RedirectToAction(nameof(GetAdmins));
        }

        [HttpPost]
        public async Task<IActionResult> AnnounceAdmin(string? id)
        {
            AppUser user = await _db.Users.FindAsync(id);

            if (!user.IsAdmin)
            {
                user.IsAdmin = true;
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            await _userManager.RemoveFromRoleAsync(user, "User");
            return RedirectToAction(nameof(GetUsers));
        }
    }
}
