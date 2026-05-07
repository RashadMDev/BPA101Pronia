using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using BPA101Pronia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPA101Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            // Sliders from DB
            List<Slider> sliders = _db.Sliders
                .Where(s => !s.IsDeleted)
                .ToList();

            // Products from DB
            List<Product> products = _db.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Images)
                .ToList();

            HomeVM vM = new HomeVM()
            {
                Products = products,
                Sliders = sliders
            };

            return View(vM);
        }

        public IActionResult Details(int id)
        {
            Product singleProduct = _db.Products
                .Include(p => p.Images)
                .Include(p => p.Reviews.Where(r => !r.IsDeleted))
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .FirstOrDefault(p => p.Id == id);

            return View(singleProduct);
        }
    }
}