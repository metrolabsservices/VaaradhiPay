using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class TetherWallet
    {
        [Key]
        public int WalletId { get; set; } // Unique identifier
        public string WalletType { get; set; } // Wallet type (e.g., BEP20, ERC20, TRC20)
        public string WalletName { get; set; } // Name of the wallet
        public string WalletAddress { get; set; } // Wallet address
        public bool IsActive { get; set; } = true; // Indicates if the wallet is active
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


        [Required]
        public string UserId { get; set; } // Foreign key to ApplicationUser
        
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } // Navigation property
    }
}
