using System;
using System.ComponentModel.DataAnnotations;

namespace VaaradhiPay.Data
{
    public class AdminUpiAccount
    {
        [Key]
        public int Id { get; set; } 
        public string UpiId { get; set; } // The UPI ID associated with this admin account
        public string AccountHolderName { get; set; } 
        public decimal Balance { get; set; }
        public string? TransactionReference { get; set; }
        public AdminCurrencyType CurrencyType { get; set; } = AdminCurrencyType.INR; // Currency type, defaulting to INR
        public AdminUpiStatus Status { get; set; } = AdminUpiStatus.Active; // Account status (e.g., Active, Inactive)
        public bool IsDeleted { get; set; } = false; 
        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow; 
        public DateTime? UpdatedOn { get; set; } 
    }

    public enum AdminUpiStatus
    {
        Active,
        Inactive,
        Suspended
    }

    public enum AdminCurrencyType
    {
        INR,
        AED,
        USD
    }
}
