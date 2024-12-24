using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Threading.Tasks;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task UploadFileAsync(string bucketName, string objectName, Stream data, string contentType);
        Task<Stream> DownloadFileAsync(string bucketName, string objectName);
        Task<bool> FileExistsAsync(string bucketName, string objectName);
        Task DeleteFileAsync(string bucketName, string objectName);
        Task<string> GeneratePresignedUrlAsync(string bucketName, string fileName, int expirationInSeconds = 3600);
        Task<string> UploadingFilesForKYC(KeyValuePair<string, FilePreviewDTO> file, string DocumentType, int? count);
        Task<string> UploadingFiles(IBrowserFile selectedFile, string DocumentType);
    }
}
