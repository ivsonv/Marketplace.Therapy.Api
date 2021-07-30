using Marketplace.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Marketplace.Services.Service
{
    public class UploadService
    {
        private readonly Domain.Interface.Integrations.Storage.IStorage _IStorage;
        public UploadService(Domain.Interface.Integrations.Storage.IStorage IStorage)
        {
            _IStorage = IStorage;
        }

        public async Task<Domain.Models.Response.upload.uploadRs> Image(IFormFile file, string key, string prefixe = "image")
        {
            var dto = new Domain.Models.dto.storage.FileDto()
            {
                key = prefixe.IsEmpty() ? $"{key}" : $"{prefixe}/{key}",
                file = file,
            };
            await _IStorage.UploadFile(dto);
            return new Domain.Models.Response.upload.uploadRs() { file = dto };
        }
    }
}