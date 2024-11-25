using System.IO;
using System.Threading.Tasks;
using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly IBucketManager _bucketManager;
        private readonly ILogger<FileStorageService> _logger;
        private readonly string _defaultBucket;

        public FileStorageService(IMinioClient minioClient, IBucketManager bucketManager, ILogger<FileStorageService> logger, IConfiguration configuration)
        {
            _minioClient = minioClient;
            _bucketManager = bucketManager;
            _logger = logger;

            // Load default bucket based on the environment
            //var environment = configuration["ASPNETCORE_ENVIRONMENT"];
            //_defaultBucket = environment == "Development"
            //    ? configuration["Minio:Environment:DeveloperBucket"]
            //    : configuration["Minio:Environment:ProductionBucket"];

            _defaultBucket = configuration["Minio:Environment:DeveloperBucket"];
        }

        public async Task UploadFileAsync(string bucketName, string objectName, Stream data, string contentType)
        {
            bucketName ??= _defaultBucket; // Use provided bucket or default
            await _bucketManager.EnsureBucketExistsAsync(bucketName);

            try
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(data)
                    .WithObjectSize(data.Length)
                    .WithContentType(contentType);

                await _minioClient.PutObjectAsync(putObjectArgs);
                _logger.LogInformation($"File '{objectName}' uploaded to bucket '{bucketName}'.");
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, $"Error uploading file '{objectName}' to bucket '{bucketName}'.");
                throw;
            }
        }

        public async Task<Stream> DownloadFileAsync(string bucketName, string objectName)
        {
            bucketName ??= _defaultBucket; // Use provided bucket or default
            var stream = new MemoryStream();

            try
            {
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithCallbackStream(streamToWrite => streamToWrite.CopyTo(stream));

                await _minioClient.GetObjectAsync(getObjectArgs);
                stream.Position = 0;
                _logger.LogInformation($"File '{objectName}' downloaded from bucket '{bucketName}'.");
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, $"Error downloading file '{objectName}' from bucket '{bucketName}'.");
                throw;
            }
            return stream;
        }

        public async Task<bool> FileExistsAsync(string bucketName, string objectName)
        {
            bucketName ??= _defaultBucket; // Use provided bucket or default
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);

                await _minioClient.StatObjectAsync(statObjectArgs);
                return true;
            }
            catch (ObjectNotFoundException)
            {
                return false;
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, $"Error checking file '{objectName}' existence in bucket '{bucketName}'.");
                throw;
            }
        }

        public async Task DeleteFileAsync(string bucketName, string objectName)
        {
            bucketName ??= _defaultBucket; // Use provided bucket or default
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);

                await _minioClient.RemoveObjectAsync(removeObjectArgs);
                _logger.LogInformation($"File '{objectName}' deleted from bucket '{bucketName}'.");
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, $"Error deleting file '{objectName}' from bucket '{bucketName}'.");
                throw;
            }
        }

        public async Task<string> GeneratePresignedUrlAsync(string bucketName, string fileName, int expirationInSeconds = 3600)
        {
            bucketName ??= _defaultBucket; // Use provided bucket or default
            try
            {
                var presignedUrl = await _minioClient.PresignedGetObjectAsync(
                    new PresignedGetObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(fileName)
                        .WithExpiry(expirationInSeconds));

                return presignedUrl;
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, $"Error generating presigned URL for '{fileName}' in bucket '{bucketName}'.");
                throw;
            }
        }
    }
}
