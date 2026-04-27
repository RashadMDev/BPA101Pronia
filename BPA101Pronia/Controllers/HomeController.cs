using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using BPA101Pronia.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
            List<Product> products = new List<Product>
                {
                    new Product { Id = 1, Name = "Apple iPhone 13 128GB", Price = 1299.99m, ImageUrl = "1-1-570x633.jpg" },
                    new Product { Id = 2, Name = "Samsung Galaxy S22", Price = 1199.50m, ImageUrl = "1-2-570x633.jpg" },
                    new Product { Id = 3, Name = "Sony WH-1000XM5 Headphones", Price = 399.00m, ImageUrl = "1-3-570x633.jpg" },
                    new Product { Id = 4, Name = "Dell XPS 13 Laptop", Price = 1899.99m, ImageUrl = "1-4-570x633.jpg" },
                    new Product { Id = 5, Name = "Apple Watch Series 8", Price = 599.00m, ImageUrl = "1-2-570x633.jpg" },
                    new Product { Id = 6, Name = "Canon EOS R6 Camera", Price = 2499.00m, ImageUrl = "1-3-570x633.jpg" },
                    new Product { Id = 7, Name = "Nike Air Max 270", Price = 199.99m, ImageUrl = "1-4-570x633.jpg" },
                    new Product { Id = 8, Name = "Adidas Ultraboost 22", Price = 179.50m, ImageUrl = "1-2-570x633.jpg" }
                };

            List<Slider> sliders = _db.Sliders.ToList();

            HomeVM vM = new HomeVM()
            {
                Products = products,
                Sliders = sliders
            };

            #region Add to db
            //Slider slider = new Slider()
            //{
            //    Title = "New Plant Soltan",
            //    Discount = 65,
            //    Desc = "Pronia, With 100% Natural, Organic & Plant Shop.",
            //    ImageUrl = "1-1-524x617.png"
            //};

            //Slider slider1 = new Slider()
            //{
            //    Title = "New Plant Soltan",
            //    Discount = 65,
            //    Desc = "Pronia, With 100% Natural, Organic & Plant Shop.",
            //    ImageUrl = "1-1-524x617.png"
            //};

            //_db.Sliders.Add(slider);
            //_db.Sliders.Add(slider1);
            //_db.SaveChanges(); 
            #endregion

            return View(vM);
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
