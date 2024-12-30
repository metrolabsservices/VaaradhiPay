using System;
using System.ComponentModel.DataAnnotations;

namespace VaaradhiPay.Data
{
    public class AdminTetherWalletAccount
    {
        [Key]
        public int Id { get; set; } 
        public string WalletAddress { get; set; } // Blockchain wallet address for transactions
        public decimal Balance { get; set; } = 0; 
        public string CurrencyType { get; set; } // Cryptocurrency type (e.g., USDT)
        public string Network { get; set; } // Blockchain network (e.g., ERC20, TRC20)
        public decimal MinimumTransferAmount { get; set; } // Minimum allowable transfer amount
        public decimal? MaximumTransferAmount { get; set; } // Optional maximum transfer limit
        public AdminWalletStatus Status { get; set; } = AdminWalletStatus.Active; // Wallet status (e.g., Active, Inactive)
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } 
    }

    public enum AdminWalletStatus
    {
        Active,
        Inactive,
        Suspended
    }
}
