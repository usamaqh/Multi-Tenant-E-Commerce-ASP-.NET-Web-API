using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Multi_Tenant_E_Commerce_API.Helpers
{
    public static class ImageUploadHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static async Task<string?> UploadImage(IFormFile image)
        {
            string ext = Path.GetExtension(image.FileName).ToLower();
            string[] allowed = new[] { ".jpg", ".png", ".jpeg" };

            if (!allowed.Contains(ext))
                return null;

            //string fileName = Guid.NewGuid() + ext;
            //string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            //if (!Directory.Exists(folderPath))
            //    Directory.CreateDirectory(folderPath);

            //string filePath = Path.Combine(folderPath, fileName);

            //using (FileStream stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await image.CopyToAsync(stream);
            //}

           // return "uploads/" + fileName;

            string connectionString = _configuration["AzureStorage:ConnectionString"]!;
            string containerName = _configuration["AzureStorage:ContainerName"]!;

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            string fileName = Guid.NewGuid() + ext;
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            using Stream stream = image.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });

            return blobClient.Uri.ToString();
        }
    }
}
