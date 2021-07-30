using Microsoft.AspNetCore.Http;

namespace Marketplace.Domain.Models.dto.storage
{
    public class FileDto
    {
        public string key { get; set; }
        public string rules { get; set; } = "public";
        public IFormFile file { get; set; }
        public Helpers.Enumerados.UploadFileType fileType { get; set; }
    }
}
