namespace VaaradhiPay.DTOs
{
    public class ErrorHandleDTO
    {
        public string Message { get; set; } = "NO ERROR";
        public string? TechnicalMessage { get; set; }
        public bool IsError { get; set; } = false;

    }
}
