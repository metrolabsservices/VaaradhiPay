using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;
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

        public async Task<(AdminTetherWalletAccount? Account, ErrorHandleDTO Error)> GetRandomAdminTetherWalletAccountAsync(string currencyType)
        {
            try
            {
                // Fetch active tether wallet accounts with the specified currency type
                var activeAccounts = await _context.AdminTetherWalletAccounts
                    .Where(a => a.Status == AdminWalletStatus.Active &&
                                !a.IsDeleted &&
                                a.CurrencyType == currencyType)
                    .ToListAsync();

                if (activeAccounts.Any())
                {
                    // Select a random account
                    var random = new Random();
                    var randomAccount = activeAccounts[random.Next(activeAccounts.Count)];
                    return (randomAccount, new ErrorHandleDTO { IsError = false });
                }

                // No accounts found for the given currency type
                return (null, new ErrorHandleDTO
                {
                    IsError = true,
                    Message = $"No active wallet account found for currency type: {currencyType}"
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return (null, new ErrorHandleDTO
                {
                    IsError = true,
                    Message = "An error occurred while fetching the wallet account.",
                    TechnicalMessage = ex.Message
                });
            }
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
