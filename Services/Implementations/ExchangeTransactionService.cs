using VaaradhiPay.Data;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Implementations
{
    public class ExchangeTransactionService
    {
        public ExchangeTransactionDTO? CurrentTransaction { get; set; }

        public SelectedBankDTO? SelectedBank { get; set; }
        public LoginCredentialsDTO? LoginCredentials { get; set; }
        public LoggedInUserDTO LoggedInInfo { get; set; }
    }
}
