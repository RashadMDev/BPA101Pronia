using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using Microsoft.AspNetCore.Mvc;

namespace BPA101Pronia.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _db;
        public SliderController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _db.Sliders.ToList();
            return View(sliders);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Slider slider)
        {
            _db.Sliders.Add(slider);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
