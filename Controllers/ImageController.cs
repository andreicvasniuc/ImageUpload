using ImageUpload.Models;
using ImageUpload.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace ImageUpload.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IImageStorageService _imageStorageService;

        public ImageController(ILogger<ImageController> logger, IImageStorageService imageStorageService)
        {
            _logger = logger;
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
