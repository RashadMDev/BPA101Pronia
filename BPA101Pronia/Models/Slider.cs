using BPA101Pronia.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BPA101Pronia.Models
{
    public class Slider : BaseEntity
    {
        [Required(ErrorMessage = "Title is required")]
        [
            StringLength(50, ErrorMessage = "Title cannot be longer than 50 characters"),
            MinLength(3, ErrorMessage = "Title must be at least 3 characters long")
        ]
        public string Title { get; set; }
        [Required(ErrorMessage = "Discount is required")]
        [Range(0, 100, ErrorMessage = "Discount range must be between 0-100")]
        public int Discount { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [
            StringLength(250, ErrorMessage = "Description cannot be longer than 250 characters"),
            MinLength(5, ErrorMessage = "Description must be at least 5 characters long")
        ]
        public string Desc { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
