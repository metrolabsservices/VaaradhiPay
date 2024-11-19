using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace VaaradhiPay.Data
{
    public class CoinType
    {
        [Key]
        public int CoinTypeId { get; set; } // Unique identifier for coin types

        public string Name { get; set; } // Full name of the cryptocurrency (e.g., Tether, Bitcoin)

        public string Symbol { get; set; } // Short symbol (e.g., USDT, BTC)

        public bool IsActive { get; set; } = true; // Determines if the coin can be used in transactions

        public bool IsDeleted { get; set; } = false; // Flag for soft deletion of coin type

        public ICollection<Transaction> Transactions { get; set; } // Related transactions

    }
}
