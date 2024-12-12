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
        public string DocumentType { get; set; } // Document type (e.g., Passport, Aadhaar, PhoneNumber, DrivingLicense)

        [Required]
        [MaxLength(50)]
        public string DocumentNumber { get; set; } // Document number( + 78921465754, 1 , 2 )

        public string? DocumentFilePath { get; set; } // File path for uploaded document

        public bool IsVerified { get; set; } = false; // Indicates if the KYC is verified

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public string UserId { get; set; } // Foreign key to ApplicationUser

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } // Navigation property


    }
}
