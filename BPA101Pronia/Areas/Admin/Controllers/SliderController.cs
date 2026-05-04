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
        #region Get Sliders
        public IActionResult Index()
        {
            List<Slider> sliders = _db.Sliders.ToList();
            return View(sliders);
        }
        #endregion

        #region Add Actions
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Slider slider)
        {
            if (!ModelState.IsValid) return View();
            _db.Sliders.Add(slider);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        } 
        #endregion

        #region Hard Delete
        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    Slider slider = _db.Sliders.Find(id);
        //    _db.Sliders.Remove(slider);
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //} 
        #endregion

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Slider slider = _db.Sliders.Find(id);
            slider.IsDeleted = true;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Restore(int id)
        {
            Slider slider = _db.Sliders.Find(id);
            slider.IsDeleted = false;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            Slider slider = _db.Sliders.Find(id);
            return View(slider);
        }

        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            Slider oldSlider = _db.Sliders.Find(slider.Id);
            oldSlider.Title = slider.Title;
            oldSlider.Desc = slider.Desc;
            oldSlider.Discount = slider.Discount;
            oldSlider.ImageUrl = slider.ImageUrl;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
