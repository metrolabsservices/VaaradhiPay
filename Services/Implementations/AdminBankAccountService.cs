using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    /// <summary>
    /// Service to manage Admin Bank Accounts.
    /// </summary>
    public class AdminBankAccountService : IAdminBankAccountService
    {
        private readonly ApplicationDbContext _context;

        public AdminBankAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminBankAccount>> GetPaginatedAdminBankAccountsAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.AdminBankAccounts
                .Where(a => !a.IsDeleted &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             a.BankName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                             a.AccountHolder.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(a => a.UpdatedDateTime);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<AdminBankAccount?> GetBankAccountByIdAsync(int id)
        {
            return await _context.AdminBankAccounts
                .Where(a => a.Id == id && !a.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AdminBankAccount>> GetActiveBankAccountsAsync()
        {
            return await _context.AdminBankAccounts
                .Where(a => a.BankAvailability == "Active" && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task AddBankAccountAsync(AdminBankAccount bankAccount)
        {
            if (bankAccount == null) throw new ArgumentNullException(nameof(bankAccount));

            _context.AdminBankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBankAccountAsync(AdminBankAccount bankAccount)
        {
            var existingAccount = await _context.AdminBankAccounts.FindAsync(bankAccount.Id);
            if (existingAccount == null) return;

            existingAccount.AccountNumber = bankAccount.AccountNumber;
            existingAccount.AccountHolder = bankAccount.AccountHolder;
            existingAccount.BankName = bankAccount.BankName;
            existingAccount.BranchCode = bankAccount.BranchCode;
            existingAccount.IfscCode = bankAccount.IfscCode;
            existingAccount.TotalCreditedAmount = bankAccount.TotalCreditedAmount;
            existingAccount.BankAvailability = bankAccount.BankAvailability;
            existingAccount.CurrencyType = bankAccount.CurrencyType;
            existingAccount.AccountType = bankAccount.AccountType;
            existingAccount.UpdatedDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBankAccountAsync(int id)
        {
            var bankAccount = await _context.AdminBankAccounts.FindAsync(id);
            if (bankAccount == null) return;

            bankAccount.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
        }
    }
}
