using System.Net;

namespace ImageUpload.Models
{
    public class ImageUploadResult
    {
        public string UploadUrl { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
