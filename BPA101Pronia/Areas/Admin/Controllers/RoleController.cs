using BPA101Pronia.Areas.Admin.ViewModels.Role;
using BPA101Pronia.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPA101Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(AppDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            List<IdentityRole> roles = await _db.Roles.ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleVM roleVM)
        {
            IdentityRole role = new IdentityRole()
            {
                Name = roleVM.Name
            };
            await _roleManager.CreateAsync(role);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string? id)
        {
            if (id is null) return NotFound();
            IdentityRole? role = await _db.Roles.FindAsync(id);
            if (role is null) return NotFound();

            UpdateRoleVM roleVM = new UpdateRoleVM()
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(roleVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRoleVM roleVM)
        {
            IdentityRole? role = await _db.Roles.FindAsync(roleVM.Id);
            if (role is null) return NotFound();

            role.Name = roleVM.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string? id)
        {
            IdentityRole? role = await _db.Roles.FindAsync(id);
            if (role is null) return NotFound();

            _db.Roles.Remove(role);

            return RedirectToAction(nameof(Index));
        }
    }
}