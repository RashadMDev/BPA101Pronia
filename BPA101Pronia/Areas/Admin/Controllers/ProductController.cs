using BPA101Pronia.Areas.Admin.ViewModels.Products;
using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPA101Pronia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products
                 .Include(p => p.Categories)
                 .Include(p => p.Tags)
                 .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            Product product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price,
                Description = productVM.Description,
                SKU = productVM.SKU
            };
            if (productVM.CategoryIds is not null)
            {
                product.Categories = await _db.Categories
                    .Where(c => productVM.CategoryIds
                    .Contains(c.Id))
                    .ToListAsync();
            }
            if (productVM.TagIds is not null)
            {
                product.Tags = await _db.Tags
                    .Where(t => productVM.TagIds
                    .Contains(t.Id))
                    .ToListAsync();
            }

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            Product product = await _db.Products.FindAsync(id);
            product.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            Product product = await _db.Products.FindAsync(id);
            product.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
