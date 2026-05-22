using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using BPA101Pronia.Utilities.ImageFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BPA101Pronia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #region Get Sliders
        public IActionResult Index()
        {
            List<Slider> sliders = _db.Sliders.ToList();
            return View(sliders);
        }
        #endregion

        #region Add Slider
        // Create view
        public IActionResult Add()
        {
            return View();
        }

        // Add to DB
        [HttpPost]
        public async Task<IActionResult> Add(Slider slider)
        {
            if (slider.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required");
                return View();
            }
            if (!slider.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Only image files are allowed");
                return View();
            }
            if (slider.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image file size must be less than 2MB");
                return View();
            }

            slider.ImageUrl = slider.ImageFile.SaveImage(_env, "uploads/sliders");
            if (!ModelState.IsValid) return View();
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Hard Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Slider slider = _db.Sliders.Find(id);
            slider.ImageUrl = slider.ImageUrl.DeleteImage(_env, "uploads/sliders");
            _db.Sliders.Remove(slider);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Soft Delete And Restore
        // Soft delete
        // [Authorize(Roles = "SuperAdmin")]
        // [HttpPost]
        // public IActionResult Delete(int id)
        // {
        //     Slider slider = _db.Sliders.Find(id);
        //     slider.IsDeleted = true;
        //     _db.SaveChanges();
        //     return RedirectToAction(nameof(Index));
        // }

        // // Restore
        // [HttpPost]
        // public IActionResult Restore(int id)
        // {
        //     Slider slider = _db.Sliders.Find(id);
        //     slider.IsDeleted = false;
        //     _db.SaveChanges();
        //     return RedirectToAction(nameof(Index));
        // }
        #endregion

        #region Update Actions
        // Update View
        public IActionResult Update(int id)
        {
            Slider slider = _db.Sliders.Find(id);
            return View(slider);
        }

        // Update to DB
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
        #endregion
    }
}
