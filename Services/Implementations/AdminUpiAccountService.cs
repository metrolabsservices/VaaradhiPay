using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaaradhiPay.Data;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class AdminUpiAccountService : IAdminUpiAccountService
    {
        private readonly ApplicationDbContext _context;

        public AdminUpiAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdminUpiAccount>> GetPaginatedUpiAccountsAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.AdminUpiAccounts
                .Where(a => !a.IsDeleted &&
                            (string.IsNullOrEmpty(searchTerm) ||
                             a.UpiId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                             a.AccountHolderName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(a => a.UpdatedOn);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<AdminUpiAccount?> GetUpiAccountByIdAsync(int id)
        {
            return await _context.AdminUpiAccounts
                .Where(a => a.Id == id && !a.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<List<AdminUpiAccount>> GetActiveUpiAccountsAsync()
        {
            return await _context.AdminUpiAccounts
                .Where(a => a.Status == AdminUpiStatus.Active && !a.IsDeleted)
                .ToListAsync();
        }

        public async Task AddUpiAccountAsync(AdminUpiAccount upiAccount)
        {
            if (upiAccount == null) throw new ArgumentNullException(nameof(upiAccount));

            _context.AdminUpiAccounts.Add(upiAccount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUpiAccountAsync(AdminUpiAccount upiAccount)
        {
            var existingAccount = await _context.AdminUpiAccounts.FindAsync(upiAccount.Id);
            if (existingAccount == null) return;

            existingAccount.UpiId = upiAccount.UpiId;
            existingAccount.AccountHolderName = upiAccount.AccountHolderName;
            existingAccount.Balance = upiAccount.Balance;
            existingAccount.TransactionReference = upiAccount.TransactionReference;
            existingAccount.CurrencyType = upiAccount.CurrencyType;
            existingAccount.Status = upiAccount.Status;
            existingAccount.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUpiAccountAsync(int id)
        {
            var upiAccount = await _context.AdminUpiAccounts.FindAsync(id);
            if (upiAccount == null) return;

            upiAccount.IsDeleted = true; // Soft delete
            await _context.SaveChangesAsync();
        }
    }
}
