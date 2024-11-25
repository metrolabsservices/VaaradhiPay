using System.Threading.Tasks;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IBucketManager
    {
        Task EnsureBucketExistsAsync(string bucketName);
    }
}
