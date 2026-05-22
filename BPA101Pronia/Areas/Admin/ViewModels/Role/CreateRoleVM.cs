using System.ComponentModel.DataAnnotations;

namespace BPA101Pronia.Areas.Admin.ViewModels.Role
{
    public record CreateRoleVM
    {
        [Required(ErrorMessage = "Role name is required")]
        public string Name { get; set; }
    }
}