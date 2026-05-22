namespace BPA101Pronia.Utilities.ImageFile
{
    public static class ImageExtension
    {
        public static string SaveImage(this IFormFile imageFile, IWebHostEnvironment env, string folder)
        {
            string path = Path.Combine(env.WebRootPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string fullPath = Path.Combine(path, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }
            return fileName; // -> image.png
        }

        public static string DeleteImage(this string imageUrl, IWebHostEnvironment env, string folder)
        {
            string path = Path.Combine(env.WebRootPath, folder);
            string fullPath = Path.Combine(path, imageUrl);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            return imageUrl;
        }
    }
}