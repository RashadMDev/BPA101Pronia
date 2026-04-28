using BPA101Pronia.Models.Base;

namespace BPA101Pronia.Models
{
    public class Slider : BaseEntity
    {
        public string Title { get; set; }
        public int Discount { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
    }
}
