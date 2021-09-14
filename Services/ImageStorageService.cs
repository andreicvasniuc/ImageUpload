using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Services
{
    public class ImageStorageService : IImageStorageService
    {
        const string ContainerName = "images";

        private readonly BlobContainerClient _containerClient;

        public ImageStorageService(string connectionString)
        {
            _containerClient = new BlobContainerClient(connectionString, blobContainerName: ContainerName);
            _containerClient.CreateIfNotExists(publicAccessType: PublicAccessType.BlobContainer);
        }

        string IImageStorageService.GenerateUploadUrl()
        {
            var blobClient = _containerClient.GetBlobClient(blobName: $"image-{DateTime.Now.Ticks}");
            return blobClient.Uri.AbsoluteUri;
        }

        async Task<int> IImageStorageService.UploadImage(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(blobName: file.FileName);

            var options = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType, ContentDisposition = file.ContentDisposition } };
            var response = await blobClient.UploadAsync(content: file.OpenReadStream(), options: options);

            return response.GetRawResponse().Status;
        }
    }
}
