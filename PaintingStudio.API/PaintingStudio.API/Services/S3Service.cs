using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PaintingStudio.API.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3Service(IConfiguration configuration)
        {
            var awsKey = configuration["AWS:AccessKey"];
            var awsSecret = configuration["AWS:SecretKey"];
            var region = configuration["AWS:Region"];

            _bucketName = configuration["AWS:BucketName"];

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(awsKey, awsSecret);
            _s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.GetBySystemName(region));
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);

            var key = $"images/{Guid.NewGuid()}-{fileName}";

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                BucketName = _bucketName,
                Key = key,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead
            };

            await fileTransferUtility.UploadAsync(uploadRequest);

            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }

        public async Task DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return;

            try
            {
                var key = fileUrl.Substring(fileUrl.IndexOf(".com/") + 5);
                await _s3Client.DeleteObjectAsync(_bucketName, key);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error deleting file from S3: {ex.Message}");
            }
        }
    }
}