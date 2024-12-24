using System.IO;
using System.Threading.Tasks;
using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using VaaradhiPay.Services.Interfaces;
using VaaradhiPay.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace VaaradhiPay.Services.Implementations
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly IBucketManager _bucketManager;
        private readonly ILogger<FileStorageService> _logger;
        private readonly ExchangeTransactionService LocalStorage;
        private readonly string _defaultBucket;

        public FileStorageService(IMinioClient minioClient, IBucketManager bucketManager, ILogger<FileStorageService> logger, IConfiguration configuration, ExchangeTransactionService localStorage)
        {
            _minioClient = minioClient;
            _bucketManager = bucketManager;
            _logger = logger;

            // Load default bucket based on the environment
            //var environment = configuration["ASPNETCORE_ENVIRONMENT"];
            //_defaultBucket = environment == "Development"
            //    ? configuration["Minio:Buckets:DeveloperBucket"]
            //    : configuration["Minio:Buckets:ProductionBucket"];

            _defaultBucket = configuration["Minio:Buckets:DeveloperBucket"];
            LocalStorage = localStorage;
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

        public async Task<string> UploadingFilesForKYC(KeyValuePair<string, FilePreviewDTO> file, string DocumentType, int? count)
        {
            try
            {
                string user = LocalStorage.LoggedInInfo.Email.Split('@')[0];
                string fileName = $"{user}_{DocumentType}_{file.Key}_{count + 1}";
                var objName = $"{user}/{DocumentType}/{fileName}";
                var buffer = Convert.FromBase64String(file.Value.PreviewUrl.Split(',')[1]);
                using var stream = new MemoryStream(buffer);
                await UploadFileAsync(null, objName, stream, file.Value.ContentType);
                return objName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<string> UploadingFiles(IBrowserFile selectedFile, string DocumentType)
        {
            try
            {
                string user = LocalStorage.LoggedInInfo.Email.Split('@')[0];
                string fileName = $"{user}_{DocumentType}";
                var objName = $"{user}/{DocumentType}/{fileName}";
                // Read the file as a stream
                using var stream = selectedFile.OpenReadStream();
                // Convert the selected file to a stream for uploading

                // Upload the file to Minio
                await UploadFileAsync(
                    null,  // Bucket name (could be configured)
                    objName,    // File name to upload
                    stream,              // File stream
                    selectedFile.ContentType // Use the file's content type (like "application/pdf", etc.)
                );
                return objName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
