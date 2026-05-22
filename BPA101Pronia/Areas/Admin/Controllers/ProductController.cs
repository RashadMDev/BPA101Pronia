using BPA101Pronia.Areas.Admin.ViewModels.Products;
using BPA101Pronia.DAL;
using BPA101Pronia.Models;
using BPA101Pronia.Utilities.ImageFile;
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
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products
            .Include(p => p.Images)
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
            if (productVM.PrimaryImage is null)
            {
                ModelState.AddModelError("PrimaryImage", "Primary Image is required");
                return View();
            }
            if (!productVM.PrimaryImage.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("PrimaryImage", "Image must be correct type");
                return View();
            }
            if (productVM.PrimaryImage.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("PrimaryImage", "Image must max 2MB");
                return View();
            }
            var primaryFileName = productVM.PrimaryImage.SaveImage(_env, "uploads/products"); // -> image.png

            Product product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price,
                Description = productVM.Description,
                SKU = productVM.SKU,
                Images = new List<Image>()
            };
            product.Images.Add(new Image
            {
                Url = primaryFileName,
                IsPrimary = true
            });

            foreach (var image in productVM.Images) // -> 3 eded sekil 
            {
                if (image is null)
                {
                    ModelState.AddModelError("PrimaryImage", "Primary Image is required");
                    return View();
                }
                if (!image.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("PrimaryImage", "Image must be correct type");
                    return View();
                }
                if (image.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("PrimaryImage", "Image must max 2MB");
                    return View();
                }
                var otherImagesFileName = image.SaveImage(_env, "uploads/products");
                product.Images.Add(new Image()
                {
                    Url = otherImagesFileName,
                    IsPrimary = false
                });
            }

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

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            Product product = await _db.Products
            .Include(p => p.Images)
            .Include(p => p.Categories)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id);


            UpdateProductVM productVM = new UpdateProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryIds = product.Categories.Select(c => c.Id).ToList(),
                TagIds = product.Tags.Select(t => t.Id).ToList(),
                OldImages = product.Images.Select(i => new ProductImagesVM
                {
                    ImgUrl = i.Url,
                    IsPrimary = i.IsPrimary
                }).ToList()
            };

            return View(productVM);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM productVM)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            if (!ModelState.IsValid) return View(productVM);

            Product existProduct = await _db.Products
            .Include(p => p.Categories)
            .Include(p => p.Tags)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == productVM.Id);


            if (productVM.PrimaryImage is not null)
            {
                if (productVM.PrimaryImage is null)
                {
                    ModelState.AddModelError("PrimaryImage", "Primary Image is required");
                    return View();
                }
                if (!productVM.PrimaryImage.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("PrimaryImage", "Image must be correct type");
                    return View();
                }
                if (productVM.PrimaryImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("PrimaryImage", "Image must max 2MB");
                    return View();
                }
                var primaryImage = existProduct.Images.FirstOrDefault(i => i.IsPrimary);

                if (primaryImage is not null)
                {
                    ImageExtension.DeleteImage(primaryImage.Url, _env, "uploads/products");
                    _db.Images.Remove(primaryImage);
                }
                existProduct.Images.Add(new Image()
                {
                    Url = productVM.PrimaryImage.SaveImage(_env, "uploads/products"),
                    IsPrimary = true
                });
            }

            if (productVM.ImageUrls is not null)
            {
                foreach (var item in existProduct.Images.Where(i => !i.IsPrimary))
                {
                    if (!productVM.ImageUrls.Any(i => i == item.Url))
                    {
                        ImageExtension.DeleteImage(item.Url, _env, "uploads/products");
                        _db.Images.Remove(item);
                    }
                }
            }
            else
            {
                foreach (var item in existProduct.Images.Where(i => !i.IsPrimary))
                {
                    ImageExtension.DeleteImage(item.Url, _env, "uploads/products");
                    _db.Images.Remove(item);
                }
            }


            if (productVM.Images is not null)
            {
                foreach (var image in productVM.Images)
                {
                    if (image is null)
                    {
                        ModelState.AddModelError("Images", "Images is required");
                    }
                    if (!image.ContentType.Contains("image/"))
                    {
                        ModelState.AddModelError("Images", "File must be an image");
                    }
                    if (image.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Images", "File size must max be 2MB");
                    }
                    existProduct.Images.Add(new Image()
                    {
                        Url = image.SaveImage(_env, "uploads/products"),
                        IsPrimary = false
                    });
                }
            }


            existProduct.Name = productVM.Name;
            existProduct.Description = productVM.Description;
            existProduct.Price = productVM.Price;

            existProduct.Categories.Clear();
            if (productVM.CategoryIds is not null)
            {
                existProduct.Categories = await _db.Categories
                .Where(c => productVM.CategoryIds
                .Contains(c.Id))
                .ToListAsync();
            }

            existProduct.Tags.Clear();
            if (productVM.TagIds is not null)
            {
                existProduct.Tags = await _db.Tags
                .Where(t => productVM.TagIds
                .Contains(t.Id))
                .ToListAsync();
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
