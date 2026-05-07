using BPA101Pronia.Models.Base;

namespace BPA101Pronia.Models
{
    public class Review : BaseEntity
    {
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Product Product { get; set; }
        public int ProductId { get; set; }

    }
}
