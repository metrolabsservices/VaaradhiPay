using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class FinancialTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        public string UserTransactionId { get; set; } // User-provided transaction ID
        public string AdminTransactionRefId { get; set; } // User-provided transaction ID
        public string PayCurrency { get; set; } // Currency paid (e.g., USD, INR)
        public string ReceiveCurrency { get; set; } // Currency received (e.g., AED)
        public bool IsBuy { get; set; }
        public decimal AmountPaid { get; set; } 
        public decimal AmountReceived { get; set; } 
        public decimal ConversionRate { get; set; } 
        public decimal ConvenienceFee { get; set; } 
        public DateTime TransactionDateTime { get; set; } = DateTime.UtcNow; 
        public string TransactionProofPath { get; set; } // Path to the uploaded proof file (e.g., screenshot)
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
        public string? TransactionNote { get; set; }

        public string UserId { get; set; } 
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }


        public int AdminBankAccountId { get; set; }
        [ForeignKey(nameof(AdminBankAccountId))]
        public AdminBankAccount AdminBankAccount { get; set; }


        public int UserBankAccountId { get; set; }
        [ForeignKey(nameof(UserBankAccountId))]
        public BankAccount UserBankAccount { get; set; }
    }

    public enum TransactionStatus
    {
        Pending,     // Transaction initiated by user, waiting for admin action
        Approved,    // Admin approved the transaction
        Declined,    // Admin rejected the transaction
        Completed,   // Payment completed successfully
        Cancelled    // User cancelled the transaction
    }
}
