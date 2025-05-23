using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

namespace BazarCarioca.WebAPI.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _S3Client;
        private readonly string _BucketName;

        public S3Service(IConfiguration config)
        {
            var awsOptions = config.GetSection("AWS");

            _S3Client = new AmazonS3Client(
                awsOptions["AccessKey"],
                awsOptions["SecretKey"],
                RegionEndpoint.GetBySystemName(awsOptions["Region"])
            );

            _BucketName = awsOptions["BucketName"];
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = file.FileName,
                BucketName = _BucketName,
                ContentType = file.ContentType
            };

            var transferUtility = new TransferUtility(_S3Client);
            await transferUtility.UploadAsync(uploadRequest);
        }
    }
}
