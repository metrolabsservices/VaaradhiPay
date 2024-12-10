namespace VaaradhiPay.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task EmailSendAsync(string email, string subject, string htmlMessage);
        Task<string> EmailConfirmationLetter();
        Task<string> SignUp();
        Task<string> KycPending();
        Task<string> KycSuccess();
        Task<string> WalletAdd();
        Task<string> BankAdd();
        Task<string> UpiAdd();
        Task<string> ChangePassword();
    }
}
