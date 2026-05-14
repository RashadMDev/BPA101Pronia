using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPA101Pronia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
