using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using BPA101Pronia.Utilities.Image;
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
        public IActionResult Add(Slider slider)
        {
            if (!slider.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be an image");
                return View();
            }
            if (!(slider.ImageFile.Length < 2 * 1024 * 1024))
            {
                ModelState.AddModelError("ImageFile", "Image file must be maximum 2MB");
                return View();
            }

            #region MyRegion
            //string path = Path.Combine(_env.WebRootPath, "uploads/sliders");
            //string fileName = slider.ImageFile.FileName;
            //string fullPath = Path.Combine(path, fileName);

            //using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            //{
            //    slider.ImageFile.CopyTo(stream);
            //}
            //slider.ImageUrl = fileName;

            #endregion
            slider.ImageUrl = slider.ImageFile.SaveImage("uploads/sliders", _env);

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

        #region Soft Delete And Restore
        // Soft delete
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Slider slider = _db.Sliders.Find(id);
            slider.IsDeleted = true;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Restore
        [HttpPost]
        public IActionResult Restore(int id)
        {
            Slider slider = _db.Sliders.Find(id);
            slider.IsDeleted = false;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
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
