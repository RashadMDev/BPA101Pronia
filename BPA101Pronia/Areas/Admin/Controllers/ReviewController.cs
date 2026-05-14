using BPA101Pronia.Areas.Admin.ViewModels.Reviews;
using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BPA101Pronia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    [Area("Admin")]
    public class ReviewController : Controller
    {
        private readonly AppDbContext _db;
        public ReviewController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Review> reviews = await _db.Reviews
                .Include(r => r.Product)
                .ToListAsync();
            return View(reviews);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _db.Products.ToListAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewVM reviewVM)
        {
            if (!ModelState.IsValid) return View();
            Review review = new Review
            {
                UserName = reviewVM.UserName,
                Content = reviewVM.Content,
                ProductId = reviewVM.ProductId,
            };
            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            Review review = await _db.Reviews.FindAsync(id);
            review.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            Review review = await _db.Reviews.FindAsync(id);
            review.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
