using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class UPIAddress
    {
        [Key]
        public int UPIAddressId { get; set; } // Unique identifier

        [Required]
        [MaxLength(100)]
        public string Address { get; set; } // UPI address (e.g., user@upi)

        public bool IsActive { get; set; } = true; // Indicates if the UPI address is active

        [Required]
        public string UserId { get; set; } // Foreign key to ApplicationUser

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } // Navigation property
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    }
}
