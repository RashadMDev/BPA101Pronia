using BPA101Pronia.Models;
using BPA101Pronia.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BPA101Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new()
            {
                UserName = registerVM.Username,
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            await _userManager.AddToRoleAsync(user, "User");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? ReturnUrl)
        {
            AppUser user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user is null)
            {
                ModelState.AddModelError("Email", "Email or password is wrong");
                return View(loginVM);
            }
            await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

            if (ReturnUrl is not null)
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
