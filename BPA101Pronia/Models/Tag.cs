using BPA101Pronia.Models.Base;

namespace BPA101Pronia.Models
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
