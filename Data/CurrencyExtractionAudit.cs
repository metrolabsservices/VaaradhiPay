using System.ComponentModel.DataAnnotations;

namespace VaaradhiPay.Data
{
    public class CurrencyExtractionAudit
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public bool IsExtractionSuccessful { get; set; } = false; // Whether the process succeeded

        public string? ErrorMessage { get; set; } // Error message if the process failed

        public int AddedCount { get; set; } = 0; // Count of new entries added

        public int UpdatedCount { get; set; } = 0; // Count of existing entries updated

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow; // Timestamp of the audit

    }
}
