using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaaradhiPay.Data;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class BankAccountService : IBankAccountService
    {
        private readonly ApplicationDbContext _context;

        public BankAccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BankAccount>> GetAllBankAccountsAsync()
        {
            return await _context.BankAccounts
                .Where(b => !b.IsDeleted)
                .OrderByDescending(b => b.UpdatedDateTime)
                .ToListAsync();
        }

        public async Task<List<BankAccount>> GetBankAccountsByUserIdAsync(string userId)
        {
            return await _context.BankAccounts
                .Where(b => b.UserId == userId && !b.IsDeleted && b.IsActive == true)
                .OrderByDescending(b => b.UpdatedDateTime)
                .ToListAsync();
        }

        public async Task<BankAccount?> GetBankAccountByIdAsync(int bankAccountId)
        {
            return await _context.BankAccounts
                .FirstOrDefaultAsync(b => b.BankAccountId == bankAccountId && !b.IsDeleted);
        }

        public async Task AddBankAccountAsync(BankAccount bankAccount)
        {
            bankAccount.CreatedDateTime = DateTime.UtcNow;
            bankAccount.UpdatedDateTime = DateTime.UtcNow;
            _context.BankAccounts.Add(bankAccount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBankAccountAsync(BankAccount bankAccount)
        {
            var existingAccount = await _context.BankAccounts.FindAsync(bankAccount.BankAccountId);
            if (existingAccount == null || existingAccount.IsDeleted) return;

            existingAccount.BankName = bankAccount.BankName;
            existingAccount.AccountHolderName = bankAccount.AccountHolderName;
            existingAccount.AccountNumber = bankAccount.AccountNumber;
            existingAccount.BranchCode = bankAccount.BranchCode;
            existingAccount.IFSCCode = bankAccount.IFSCCode;
            existingAccount.ProofFilePath = bankAccount.ProofFilePath;
            existingAccount.CurrencyType = bankAccount.CurrencyType;
            existingAccount.AccountType = bankAccount.AccountType;
            existingAccount.IsActive = bankAccount.IsActive;
            existingAccount.IsVerified = bankAccount.IsVerified;
            existingAccount.UpdatedDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBankAccountAsync(int bankAccountId)
        {
            var existingAccount = await _context.BankAccounts.FindAsync(bankAccountId);
            if (existingAccount == null || existingAccount.IsDeleted) return;

            existingAccount.IsDeleted = true;
            existingAccount.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<List<BankAccount>> GetActiveBankAccountsByCurrencyAsync(string currencyType = null)
        {
            var query = _context.BankAccounts
                .Where(b => b.IsActive && !b.IsDeleted);

            if (!string.IsNullOrEmpty(currencyType))
            {
                query = query.Where(b => b.CurrencyType == currencyType);
            }

            return await query.OrderByDescending(b => b.UpdatedDateTime).ToListAsync();
        }

        public async Task<List<BankAccount>> GetActiveBankAccountsByCurrencyUserIdAsync(string currencyType = null, string userId = null)
        {
            var query = _context.BankAccounts
                .Where(b => b.IsActive && !b.IsDeleted);

            if (!string.IsNullOrEmpty(currencyType))
            {
                query = query.Where(b => b.CurrencyType == currencyType && b.UserId == userId);
            }

            return await query.OrderByDescending(b => b.UpdatedDateTime).ToListAsync();
        }
        
    }
}
