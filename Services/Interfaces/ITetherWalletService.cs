using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services.Interfaces
{
    public interface ITetherWalletService
    {
        Task<List<TetherWallet>> GetPaginatedTetherWalletsAsync(string userId, string searchTerm, int page, int pageSize);
        Task<TetherWallet?> GetTetherWalletByIdAsync(int id);
        Task<List<TetherWallet>> GetActiveTetherWalletsByUserAsync(string userId);
        Task AddTetherWalletAsync(TetherWallet tetherWallet);
        Task UpdateTetherWalletAsync(TetherWallet tetherWallet);
        Task DeleteTetherWalletAsync(int id);
    }
}
