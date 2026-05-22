
using System.ComponentModel.DataAnnotations;

namespace BPA101Pronia.Areas.Admin.ViewModels.Products
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Desc is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "SKU is required")]
        public string SKU { get; set; }
        public IFormFile PrimaryImage { get; set; }
        public List<IFormFile> Images { get; set; }
        [Required(ErrorMessage = "Categories must be chosen")]
        public List<int> CategoryIds { get; set; }
        [Required(ErrorMessage = "Categories must be chosen")]
        public List<int> TagIds { get; set; }
    }
}
