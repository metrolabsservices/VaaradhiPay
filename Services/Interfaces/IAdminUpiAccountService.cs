using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IAdminUpiAccountService
    {
        Task<List<AdminUpiAccount>> GetPaginatedUpiAccountsAsync(string searchTerm, int page, int pageSize);
        Task<AdminUpiAccount?> GetUpiAccountByIdAsync(int id);
        Task<List<AdminUpiAccount>> GetActiveUpiAccountsAsync();
        Task<(AdminUpiAccount? Account, ErrorHandleDTO Error)> GetRandomAdminUpiAccountAsync(string currencyType);
        Task AddUpiAccountAsync(AdminUpiAccount upiAccount);
        Task UpdateUpiAccountAsync(AdminUpiAccount upiAccount);
        Task DeleteUpiAccountAsync(int id);
    }
}
