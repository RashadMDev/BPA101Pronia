using BPA101Pronia.Models.Base;

namespace BPA101Pronia.Models
{
    public class Image : BaseEntity
    {
        public string Url { get; set; }
        public bool IsPrimary { get; set; }
        public Product Product { get; set; }
    }
}
