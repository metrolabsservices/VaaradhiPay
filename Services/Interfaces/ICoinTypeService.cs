using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services.Interfaces
{
    public interface ICoinTypeService
    {
        Task<List<CoinType>> GetPaginatedCoinTypesAsync(string searchTerm, int page, int pageSize);
        Task<CoinType> GetCoinTypeByIdAsync(int id);
        Task<List<CoinType>> GetActiveCoinsAsync();
        Task AddCoinTypeAsync(CoinType coinType);
        Task UpdateCoinTypeAsync(CoinType coinType);
        Task DeleteCoinTypeAsync(int id);
        Task BulkAddCoinTypesAsync(string jsonFilePath);

    }
}
