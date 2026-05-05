using BPA101Pronia.Models;

namespace BPA101Pronia.Utilities.Image
{
    public static class FileUpload
    {
        public static string SaveImage(this IFormFile imageFile, string folder, IWebHostEnvironment env)
        {
            string path = Path.Combine(env.WebRootPath, folder);
            string fileName = Guid.NewGuid().ToString() + imageFile.FileName;
            string fullPath = Path.Combine(path, fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }
            return fileName;
        }
    }
}
