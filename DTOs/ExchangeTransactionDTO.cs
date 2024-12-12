namespace VaaradhiPay.DTOs
{
    public class ExchangeTransactionDTO
    {
        public decimal YouPay { get; set; }
        public string PayCurrency { get; set; } = string.Empty;
        public string ReceiveCurrency { get; set; } = string.Empty;
        public decimal YouReceive { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool IsBuy { get; set; }
    }
}
