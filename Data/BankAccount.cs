using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; } 

        public string BankName { get; set; } 

        public string AccountHolderName { get; set; } 

        public string AccountNumber { get; set; }

        public string? BranchCode { get; set; }

        public string? IFSCCode { get; set; } 

        public string ProofFilePath { get; set; } 

        public string CurrencyType { get; set; }  // USD - INR - AED 

        public string AccountType { get; set; }  // Savings - Business - Checking.

        public bool IsActive { get; set; } = true;

        public bool IsVerified { get; set; } = false;

        public DateTime UpdatedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public string UserId { get; set; } 

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public ICollection<FinancialTransaction> TransactionRecords { get; set; }

    }
}
