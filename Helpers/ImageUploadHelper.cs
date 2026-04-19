namespace Multi_Tenant_E_Commerce_API.Helpers
{
    public static class ImageUploadHelper
    {
        public static async Task<string?> UploadImage(IFormFile image)
        {
            string ext = Path.GetExtension(image.FileName).ToLower();
            string[] allowed = new[] { ".jpg", ".png", ".jpeg" };

            if (!allowed.Contains(ext))
                return null;

            string fileName = Guid.NewGuid() + ext;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return "uploads/" + fileName;
        }
    }
}
