using Microsoft.AspNetCore.Components.Forms;
using Microsoft.VisualBasic.FileIO;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;
using VaaradhiPay.Services.Implementations;
using VaaradhiPay.Services.Interfaces;
using static VaaradhiPay.Components.Pages.UserView.UserProfilePages.UserProfileBlocks.KycUploadingBlock;

namespace VaaradhiPay.Components.Pages.TestingByChandu
{
    public class TestingMinioService
    {
        private readonly IFileStorageService fileStorage;
        private readonly ExchangeTransactionService LocalStorage;
        public TestingMinioService(IFileStorageService file, ExchangeTransactionService localStorage)
        {
            fileStorage = file;
            LocalStorage = localStorage;
        }

        public async Task<string> UploadingFiles(KeyValuePair<string, FilePreviewDTO> file, string DocumentType, int? count)
        {
            try
            {
                string user = LocalStorage.LoggedInInfo.Email.Split('@')[0];
                string fileName = $"{user}_{DocumentType}_{file.Key}_{count + 1}";
                var objName = $"{user}/{DocumentType}/{fileName}";
                var buffer = Convert.FromBase64String(file.Value.PreviewUrl.Split(',')[1]);
                using var stream = new MemoryStream(buffer);
                await fileStorage.UploadFileAsync(null, objName, stream, file.Value.ContentType);
                return objName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<string> upload(IBrowserFile selectedFile,string DocumentType)
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
                await fileStorage.UploadFileAsync(
                    null,  // Bucket name (could be configured)
                    objName,    // File name to upload
                    stream,              // File stream
                    selectedFile.ContentType // Use the file's content type (like "application/pdf", etc.)
                );
                return objName;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
