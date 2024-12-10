namespace VaaradhiPay.DTOs
{
    public class ExchangeRateDTO
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Rate { get; set; }
    }
}
