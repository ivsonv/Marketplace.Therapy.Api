using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Marketplace.Integrations.Storage.Amazon
{
    public class AmazonStorageClient
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _amazonS3 = null;
        private readonly RegionEndpoint _region;

        public AmazonStorageClient(IConfiguration configuration)
        {
            _configuration = configuration;
            switch (_configuration["storage:amazon:region"])
            {
                case "sa-east-1": _region = RegionEndpoint.SAEast1; break;
                default:
                    _region = RegionEndpoint.USEast1; break;
            }

            this._amazonS3 = new AmazonS3Client(awsAccessKeyId: configuration["storage:amazon:accesskeyid"],
                                                awsSecretAccessKey: configuration["storage:amazon:secretaccesskey"],
                                                region: _region);
        }

        public async Task Upload(Domain.Models.dto.storage.FileDto fileDto)
        {
            try
            {
                await _amazonS3.PutObjectAsync(new PutObjectRequest()
                {
                    BucketName = _configuration["storage:amazon:bucket"],
                    InputStream = fileDto.file.OpenReadStream(),
                    CannedACL = this.GetACL(fileDto.rules),
                    Key = fileDto.key,
                });
            }
            catch (Exception) { throw; }
        }

        public async Task Remove(Domain.Models.dto.storage.FileDto fileDto)
        {
            try
            {
                await _amazonS3.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = _configuration["storage:amazon:bucket"],
                    Key = fileDto.key
                });
            }
            catch (Exception) { throw; }
        }

        private S3CannedACL GetACL(string rules)
        {
            switch (rules)
            {
                case "public": return S3CannedACL.PublicRead;
                default: return S3CannedACL.Private;
            }
        }
    }
}
