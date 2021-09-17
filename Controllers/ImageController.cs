using ImageUpload.Models;
using ImageUpload.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace ImageUpload.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageStorageService _imageStorageService;

        public ImageController(IImageStorageService imageStorageService)
        {
            _imageStorageService = imageStorageService;
        }

        [HttpGet("geturl")]
        public ImageUploadResult GetUrl()
        {
            return new ImageUploadResult {
                UploadUrl = _imageStorageService.GenerateUploadUrl(),
                StatusCode = HttpStatusCode.OK
            };
        }

        [HttpPost("upload")]
        public async Task<int?> Upload(IFormFile file) => await _imageStorageService.UploadImage(file);
    }
}
