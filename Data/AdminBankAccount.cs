using System.ComponentModel.DataAnnotations;

namespace VaaradhiPay.Data
{
    public class AdminBankAccount
    {
        [Key]
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public string BankName { get; set; }
        public string? BranchCode { get; set; } // Branch code of the bank (used in specific countries, e.g., Sort Code, BSB Number)
        public string? IfscCode { get; set; } // IFSC Code for the bank branch (used primarily in India for electronic transfers)
        public decimal TotalCreditedAmount { get; set; } = 0;
        public string BankAvailability { get; set; } = "Active"; // In-Active - Active
        public string CurrencyType { get; set; }  // USD - INR - AED 
        public string AccountType { get; set; }  // Savings - Business - Checking.
        public DateTime UpdatedDateTime { get; set; } 
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public ICollection<FinancialTransaction> TransactionRecords { get; set; }
    }
}



