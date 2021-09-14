using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ImageUpload.Services
{
    public interface IImageStorageService
    {
        string GenerateUploadUrl();

        Task<int> UploadImage(IFormFile file);
    }
}
