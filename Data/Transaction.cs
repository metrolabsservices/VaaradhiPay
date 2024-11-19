using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; } // Unique identifier for the transaction

        public string ReferenceNumber { get; set; } // Reference number of the transaction

        public int CoinTypeId { get; set; } // Foreign key to CoinType

        [ForeignKey("CoinTypeId")]
        public CoinType CoinType { get; set; } // Navigation property for CoinType

        public string TransactionType { get; set; } // "Buy" or "Sell"

        public decimal Volume { get; set; } // Positive (buy) or negative (sell) volume

        public decimal Amount { get; set; } // Local currency equivalent of the transaction

        public DateTime TransactionDateTime { get; set; } // Date and time of the transaction

        public bool IsDeleted { get; set; } = false; // Soft delete flag
    }
}
