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

        public async Task<Domain.Models.Response.upload.uploadRs> UploadImage(IFormFile file, string key, string prefixe)
        {
            var dto = new Domain.Models.dto.storage.FileDto()
            {
                key = $"image/{prefixe}/{key}",
                file = file,
            };
            return await this.UploadFile(dto);
        }

        public async Task<Domain.Models.Response.upload.uploadRs> RemoveImage(string key, string prefixe)
        {
            return await this.RemoveFile(new Domain.Models.dto.storage.FileDto()
            {
                key = $"image/{prefixe}/{key}",
            });
        }

        private async Task<Domain.Models.Response.upload.uploadRs> RemoveFile(Domain.Models.dto.storage.FileDto dto)
        {
            await _IStorage.RemoveFile(dto);
            return new Domain.Models.Response.upload.uploadRs() { file = dto };
        }
        private async Task<Domain.Models.Response.upload.uploadRs> UploadFile(Domain.Models.dto.storage.FileDto dto)
        {
            await _IStorage.UploadFile(dto);
            return new Domain.Models.Response.upload.uploadRs() { file = dto };
        }
    }
}