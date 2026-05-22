
using System.ComponentModel.DataAnnotations;

namespace BPA101Pronia.Areas.Admin.ViewModels.Role
{
    public record UpdateRoleVM
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Role name is required")]
        public string Name { get; set; }
    }
}