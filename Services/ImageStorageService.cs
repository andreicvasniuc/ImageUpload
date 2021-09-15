using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ImageUpload.Services
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly BlobContainerClient _container;
        private readonly ILogger<ImageStorageService> _logger;

        public ImageStorageService(string connectionString, ILogger<ImageStorageService> logger)
        {
            _logger = logger;
            _container = InstantiateImagesContainer(connectionString);
        }

        string IImageStorageService.GenerateUploadUrl()
        {
            if (_container == null) return null;

            var blobClient = _container.GetBlobClient(blobName: $"image-{DateTime.Now.Ticks}");
            return blobClient.Uri.AbsoluteUri;
        }

        async Task<int?> IImageStorageService.UploadImage(IFormFile file)
        {
            if (file is null) throw new ArgumentNullException(nameof(file));
            if (_container == null) return null;

            var blobClient = _container.GetBlobClient(blobName: file.FileName);

            var options = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType, ContentDisposition = file.ContentDisposition } };
            var response = await blobClient.UploadAsync(content: file.OpenReadStream(), options: options);

            return response.GetRawResponse().Status;
        }

        private BlobContainerClient InstantiateImagesContainer(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Can't be empty", nameof(connectionString));

                var container = new BlobContainerClient(connectionString, blobContainerName: "images");
                container.CreateIfNotExists(publicAccessType: PublicAccessType.BlobContainer);
                return container;
            } catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return null;
            }
        }
    }
}
