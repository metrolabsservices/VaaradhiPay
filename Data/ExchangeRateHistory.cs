namespace VaaradhiPay.Data
{
    public class ExchangeRateHistory
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ModifiedBy { get; set; }
        public string ModifiedID { get; set; }
        public decimal BuyRateNafa { get; set; }
        public decimal SellRateNafa { get; set; }
        public decimal BuyRateUsdt { get; set; }
        public decimal SellRateUsdt { get; set; }
        public decimal? TaxAmountNafa { get; set; } = 0;
        public decimal? TaxAmountUsdt { get; set; } = 0;
        public decimal? ExchangePercentageNafa { get; set; }
        public decimal? ExchangePercentageUsdt { get; set; }
        public decimal CurrentInrRate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
