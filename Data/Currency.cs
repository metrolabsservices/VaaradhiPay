using System.ComponentModel.DataAnnotations;

namespace VaaradhiPay.Data
{
    public class Currency
    {
        [Key]
        public int Id { get; set; } // Primary Key

        public string Name { get; set; } = string.Empty; // Lowercase currency name

        public string ISOCode { get; set; } // ISO code of the currency

        public decimal Rate { get; set; } // Exchange rate relative to USD

        public DateTime LastUpdated { get; set; } // Timestamp for when the rate was last updated

    }
}
