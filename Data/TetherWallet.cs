using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaaradhiPay.Data
{
    public class TetherWallet
    {
        [Key]
        public int WalletId { get; set; } // Unique identifier

        [Required]
        [MaxLength(50)]
        public string WalletType { get; set; } // Wallet type (e.g., BEP20, ERC20, TRC20)

        [Required]
        [MaxLength(100)]
        public string WalletName { get; set; } // Name of the wallet

        [Required]
        [MaxLength(150)]
        public string WalletAddress { get; set; } // Wallet address

        public bool IsActive { get; set; } = true; // Indicates if the wallet is active

        [Required]
        public string UserId { get; set; } // Foreign key to ApplicationUser

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } // Navigation property
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    }
}
