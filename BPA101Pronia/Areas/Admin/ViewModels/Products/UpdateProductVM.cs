using System.ComponentModel.DataAnnotations;

namespace BPA101Pronia.Areas.Admin.ViewModels.Products
{
    public record UpdateProductVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Desc is required")]
        public string Description { get; set; }
        public IFormFile PrimaryImage { get; set; }
        public List<IFormFile> Images { get; set; }
        [Required(ErrorMessage = "Categories must be chosen")]
        public List<ProductImagesVM> OldImages { get; set; } = new List<ProductImagesVM>();
        public List<string> ImageUrls { get; set; }
        public List<int> CategoryIds { get; set; }
        [Required(ErrorMessage = "Categories must be chosen")]
        public List<int> TagIds { get; set; }
    }
}