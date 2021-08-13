using Marketplace.Domain.Models.dto.storage;
using System.Threading.Tasks;

namespace Marketplace.Integrations.Storage
{
    public class StorageIntegrations : Domain.Interface.Integrations.Storage.IStorage
    {
        private readonly Amazon.AmazonStorageClient _storageAmazon;
        public StorageIntegrations(Amazon.AmazonStorageClient storageAmazon)
        {
            _storageAmazon = storageAmazon;
        }

        public async Task RemoveFile(FileDto fileDto)
        {
            await _storageAmazon.Remove(fileDto);
        }

        public async Task UploadFile(FileDto fileDto)
        {
            await _storageAmazon.Upload(fileDto);
        }
    }
}
