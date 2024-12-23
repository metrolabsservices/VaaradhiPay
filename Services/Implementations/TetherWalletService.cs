using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaaradhiPay.Data;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class TetherWalletService : ITetherWalletService
    {
        private readonly ApplicationDbContext _context;

        public TetherWalletService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TetherWallet>> GetPaginatedTetherWalletsAsync(string userId, string searchTerm, int page, int pageSize)
        {
            var query = _context.TetherWallets
                .Where(w => w.UserId == userId &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             w.WalletName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(w => w.CreatedDate);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<TetherWallet?> GetTetherWalletByIdAsync(int id)
        {
            return await _context.TetherWallets
                .FirstOrDefaultAsync(w => w.WalletId == id);
        }

        public async Task<List<TetherWallet>> GetActiveTetherWalletsByUserAsync(string userId)
        {
            return await _context.TetherWallets
                .Where(w => w.UserId == userId && w.IsActive)
                .OrderBy(w => w.CreatedDate)
                .ToListAsync();
        }

        public async Task AddTetherWalletAsync(TetherWallet tetherWallet)
        {
            if (tetherWallet == null) throw new ArgumentNullException(nameof(tetherWallet));

            await _context.TetherWallets.AddAsync(tetherWallet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTetherWalletAsync(TetherWallet tetherWallet)
        {
            var existingWallet = await _context.TetherWallets.FindAsync(tetherWallet.WalletId);
            if (existingWallet == null) return;

            existingWallet.WalletType = tetherWallet.WalletType;
            existingWallet.WalletName = tetherWallet.WalletName;
            existingWallet.WalletAddress = tetherWallet.WalletAddress;
            existingWallet.IsActive = tetherWallet.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTetherWalletAsync(int id)
        {
            var wallet = await _context.TetherWallets.FindAsync(id);
            if (wallet == null) return;

            wallet.IsActive = false; // soft delete
            await _context.SaveChangesAsync();
        }
    }
}
