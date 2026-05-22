namespace BPA101Pronia.Areas.Admin.ViewModels.Products
{
    public record ProductImagesVM
    {
        public string ImgUrl { get; set; }
        public bool IsPrimary { get; set; }
    }
}