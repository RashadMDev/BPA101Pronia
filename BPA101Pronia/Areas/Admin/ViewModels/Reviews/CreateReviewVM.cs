using System.ComponentModel.DataAnnotations;

namespace BPA101Pronia.Areas.Admin.ViewModels.Reviews
{
    public class CreateReviewVM
    {
        [Required(ErrorMessage = "Username is required.")]
        [
            StringLength(15, ErrorMessage = "Username cannot be longer than 15 characters."),
            MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")
        ]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        [
            StringLength(200, ErrorMessage = "Content cannot be longer than 200 characters."),
            MinLength(3, ErrorMessage = "Content must be at least 3 characters long.")
        ]
        public string Content { get; set; }
        [Required(ErrorMessage = "Product is required.")]
        public int ProductId { get; set; }
    }
}
