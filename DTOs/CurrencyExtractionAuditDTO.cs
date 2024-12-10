namespace VaaradhiPay.DTOs
{
    public class CurrencyExtractionAuditDTO
    {
        public bool IsExtractionSuccessful { get; set; } = false; // Success or failure of extraction
        public string? ErrorMessage { get; set; } = null; // Error message if any
        public int AddedCount { get; set; } = 0; // Number of new currencies added
        public int UpdatedCount { get; set; } = 0; // Number of currencies updated
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow; // Operation timestamp

    }
}
