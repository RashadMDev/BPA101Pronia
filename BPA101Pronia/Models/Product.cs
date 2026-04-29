using BPA101Pronia.Models.Base;

namespace BPA101Pronia.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Image> Images { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
