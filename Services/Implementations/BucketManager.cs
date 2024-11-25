using Minio;
using Minio.Exceptions;
using Minio.DataModel.Args;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class BucketManager : IBucketManager
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<BucketManager> _logger;
        private readonly string _bucketName;

        public BucketManager(IMinioClient minioClient, ILogger<BucketManager> logger, IConfiguration configuration)
        {
            _minioClient = minioClient;
            _logger = logger;

            // Determine the environment-specific bucket
            var environment = configuration["ASPNETCORE_ENVIRONMENT"];
            //_bucketName = environment == "Development"
            //    ? configuration["Minio:Environment:DeveloperBucket"]
            //    : configuration["Minio:Environment:ProductionBucket"];

            _bucketName = configuration["Minio:Environment:DeveloperBucket"];
        }

        public async Task EnsureBucketExistsAsync(string bucketName = null)
        {
            var bucketToCheck = bucketName ?? _bucketName; // Use the provided bucket or default

            try
            {
                bool exists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketToCheck));
                if (!exists)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketToCheck));
                    _logger.LogInformation($"Bucket '{bucketToCheck}' created successfully.");
                }
                else
                {
                    _logger.LogInformation($"Bucket '{bucketToCheck}' already exists.");
                }
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, $"Failed to ensure bucket '{bucketToCheck}' exists.");
                throw;
            }
        }
    }
}
