using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IAdminUpiAccountService
    {
        Task<List<AdminUpiAccount>> GetPaginatedUpiAccountsAsync(string searchTerm, int page, int pageSize);
        Task<AdminUpiAccount?> GetUpiAccountByIdAsync(int id);
        Task<List<AdminUpiAccount>> GetActiveUpiAccountsAsync();
        Task AddUpiAccountAsync(AdminUpiAccount upiAccount);
        Task UpdateUpiAccountAsync(AdminUpiAccount upiAccount);
        Task DeleteUpiAccountAsync(int id);
    }
}
