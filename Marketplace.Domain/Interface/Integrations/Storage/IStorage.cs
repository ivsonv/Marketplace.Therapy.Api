using Marketplace.Domain.Models.dto.storage;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Marketplace.Domain.Interface.Integrations.Storage
{
    public interface IStorage
    {
        Task UploadFile(FileDto fileDto);
        Task RemoveFile(FileDto fileDto);
    }
}
