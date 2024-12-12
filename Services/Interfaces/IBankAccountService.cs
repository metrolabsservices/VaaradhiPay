using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task<List<BankAccount>> GetAllBankAccountsAsync();
        Task<List<BankAccount>> GetBankAccountsByUserIdAsync(string userId);
        Task<BankAccount?> GetBankAccountByIdAsync(int bankAccountId);
        Task AddBankAccountAsync(BankAccount bankAccount);
        Task UpdateBankAccountAsync(BankAccount bankAccount);
        Task DeleteBankAccountAsync(int bankAccountId); // Soft delete
        Task<List<BankAccount>> GetActiveBankAccountsByCurrencyAsync(string currencyType = null);
        Task<List<BankAccount>> GetActiveBankAccountsByCurrencyUserIdAsync(string currencyType = null, string userId = null);
    }
}
