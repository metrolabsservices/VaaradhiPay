using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IAdminTetherWalletAccountService
    {
        Task<List<AdminTetherWalletAccount>> GetPaginatedTetherWalletAccountsAsync(string searchTerm, int page, int pageSize);
        Task<AdminTetherWalletAccount?> GetTetherWalletAccountByIdAsync(int id);
        Task<List<AdminTetherWalletAccount>> GetActiveTetherWalletAccountsAsync();
        Task AddTetherWalletAccountAsync(AdminTetherWalletAccount tetherWalletAccount);
        Task UpdateTetherWalletAccountAsync(AdminTetherWalletAccount tetherWalletAccount);
        Task DeleteTetherWalletAccountAsync(int id);
    }
}
