using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; } // Unique identifier

        [Required]
        [MaxLength(100)]
        public string BankName { get; set; } // Name of the bank

        [Required]
        [MaxLength(100)]
        public string AccountHolderName { get; set; } // Account holder's name

        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; } // Bank account number

        [Required]
        [MaxLength(11)]
        public string IFSCCode { get; set; } // IFSC code of the bank

        public string? ProofFilePath { get; set; } // File path for bank proof (uploaded document)

        public bool IsActive { get; set; } = true; // Indicates if the account is active

        [Required]
        public string UserId { get; set; } // Foreign key to ApplicationUser

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } // Navigation property

        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    }
}
