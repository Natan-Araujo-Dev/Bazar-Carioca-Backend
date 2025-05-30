using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using BazarCarioca.WebAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace BazarCarioca.WebAPI.Services
{
    public class S3Service : IWebService, IS3Service
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

        public async Task<string> UploadImageAsync(string entityDirectory, string fileName, IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var filePrefix = fileName;
            CustomDate custom = new CustomDate();
            var date = custom.WithoutBars(DateTime.Now);
            var fileExtension = Path.GetExtension(file.FileName);

            var key = $"images/{entityDirectory}/{filePrefix}-{date}{fileExtension}";
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = key,
                BucketName = _BucketName,
                ContentType = file.ContentType
            };

            var transferUtility = new TransferUtility(_S3Client);
            await transferUtility.UploadAsync(uploadRequest);

            var region = _S3Client.Config.RegionEndpoint.SystemName;
            return $"https://{_BucketName}.s3.{region}.amazonaws.com/{key}";
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            // retorna apenas o que vem após "amazonaws.com/". Ou seja: a key para amazon utilizar
            var key = fileUrl.Substring(fileUrl.IndexOf("amazonaws.com/") + "amazonaws.com/".Length);

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _BucketName,
                Key = key
            };

            await _S3Client.DeleteObjectAsync(deleteRequest);

            return true;
        }
    }
}
