using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class KYCDetails
    {
        [Key]
        public int KYCId { get; set; } // Unique identifier

        [Required]
        [MaxLength(100)]
        public string DocumentType { get; set; } // Document type (e.g., Passport, Aadhaar)

        [Required]
        [MaxLength(50)]
        public string DocumentNumber { get; set; } // Document number

        public string? DocumentFilePath { get; set; } // File path for uploaded document

        public bool IsVerified { get; set; } = false; // Indicates if the KYC is verified

        [Required]
        public string UserId { get; set; } // Foreign key to ApplicationUser

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } // Navigation property

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
