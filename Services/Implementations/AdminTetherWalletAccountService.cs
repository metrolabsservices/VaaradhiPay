using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaaradhiPay.Data;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class AdminTetherWalletAccountService : IAdminTetherWalletAccountService
    {
        private readonly ApplicationDbContext _context;

        public AdminTetherWalletAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminTetherWalletAccount>> GetPaginatedTetherWalletAccountsAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.AdminTetherWalletAccounts
                .Where(a => !a.IsDeleted &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             a.WalletAddress.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                             a.CurrencyType.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(a => a.UpdatedOn);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<AdminTetherWalletAccount?> GetTetherWalletAccountByIdAsync(int id)
        {
            return await _context.AdminTetherWalletAccounts
                .Where(a => a.Id == id && !a.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AdminTetherWalletAccount>> GetActiveTetherWalletAccountsAsync()
        {
            return await _context.AdminTetherWalletAccounts
                .Where(a => a.Status == AdminWalletStatus.Active && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task AddTetherWalletAccountAsync(AdminTetherWalletAccount tetherWalletAccount)
        {
            if (tetherWalletAccount == null) throw new ArgumentNullException(nameof(tetherWalletAccount));

            _context.AdminTetherWalletAccounts.Add(tetherWalletAccount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTetherWalletAccountAsync(AdminTetherWalletAccount tetherWalletAccount)
        {
            var existingAccount = await _context.AdminTetherWalletAccounts.FindAsync(tetherWalletAccount.Id);
            if (existingAccount == null) return;

            existingAccount.WalletAddress = tetherWalletAccount.WalletAddress;
            existingAccount.Balance = tetherWalletAccount.Balance;
            existingAccount.CurrencyType = tetherWalletAccount.CurrencyType;
            existingAccount.Network = tetherWalletAccount.Network;
            existingAccount.MinimumTransferAmount = tetherWalletAccount.MinimumTransferAmount;
            existingAccount.MaximumTransferAmount = tetherWalletAccount.MaximumTransferAmount;
            existingAccount.Status = tetherWalletAccount.Status;
            existingAccount.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTetherWalletAccountAsync(int id)
        {
            var tetherWalletAccount = await _context.AdminTetherWalletAccounts.FindAsync(id);
            if (tetherWalletAccount == null) return;

            tetherWalletAccount.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
        }
    }
}
