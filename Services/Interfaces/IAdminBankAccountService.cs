using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Interfaces
{
    /// Provides methods to manage Admin Bank Accounts.
    public interface IAdminBankAccountService
    {
        Task<List<AdminBankAccount>> GetPaginatedAdminBankAccountsAsync(string searchTerm, int page, int pageSize);
        Task<AdminBankAccount?> GetBankAccountByIdAsync(int id);
        Task<List<AdminBankAccount>> GetActiveBankAccountsAsync();
        Task<(AdminBankAccount? Account, ErrorHandleDTO Error)> GetRandomAdminBankAccountAsync(string currencyType);
        Task AddBankAccountAsync(AdminBankAccount bankAccount);
        Task UpdateBankAccountAsync(AdminBankAccount bankAccount);
        Task DeleteBankAccountAsync(int id);
    }
}
