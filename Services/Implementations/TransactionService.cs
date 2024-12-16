using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get paginated transactions with filtering options
        public async Task<List<FinancialTransaction>> GetPaginatedTransactionsAsync(
            string searchTerm,
            int page,
            int pageSize,
            DateTime? startDate = null,
            DateTime? endDate = null,
            TransactionStatus? status = null)
        {
            var query = _context.FinancialTransactions
                .Include(t => t.User)
                .Include(t => t.AdminBankAccount)
                .Include(t => t.UserBankAccount)
                .Where(t => t.Status != TransactionStatus.Cancelled);

            // Apply filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t =>
                    t.UserTransactionId.Contains(searchTerm) ||
                    t.AdminTransactionRefId.Contains(searchTerm));
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.TransactionDateTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.TransactionDateTime <= endDate.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            // Pagination
            return await query
                .OrderByDescending(t => t.TransactionDateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Get transaction by ID
        public async Task<FinancialTransaction> GetTransactionByIdAsync(int id)
        {
            var transaction = await _context.FinancialTransactions
                .Include(t => t.User)
                .Include(t => t.AdminBankAccount)
                .Include(t => t.UserBankAccount)
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                throw new InvalidOperationException($"Transaction with ID {id} not found.");
            }

            return transaction;
        }

        // Add a new transaction
        public async Task AddTransactionAsync(FinancialTransaction transaction)
        {
            ValidateTransaction(transaction);

            try
            {
                _context.FinancialTransactions.Add(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while saving the transaction.", ex);
            }
        }

        // Update an existing transaction
        public async Task UpdateTransactionAsync(FinancialTransaction transaction)
        {
            ValidateTransaction(transaction);

            var existingTransaction = await _context.FinancialTransactions.FindAsync(transaction.TransactionId);

            if (existingTransaction == null)
            {
                throw new InvalidOperationException($"Transaction with ID {transaction.TransactionId} not found.");
            }

            existingTransaction.UserTransactionId = transaction.UserTransactionId;
            existingTransaction.AdminTransactionRefId = transaction.AdminTransactionRefId;
            existingTransaction.PayCurrency = transaction.PayCurrency;
            existingTransaction.ReceiveCurrency = transaction.ReceiveCurrency;
            existingTransaction.IsBuy = transaction.IsBuy;
            existingTransaction.AmountPaid = transaction.AmountPaid;
            existingTransaction.AmountReceived = transaction.AmountReceived;
            existingTransaction.ConversionRate = transaction.ConversionRate;
            existingTransaction.ConvenienceFee = transaction.ConvenienceFee;
            existingTransaction.Status = transaction.Status;
            existingTransaction.TransactionProofPath = transaction.TransactionProofPath;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while updating the transaction.", ex);
            }
        }

        // Approve or Decline a transaction
        public async Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus status)
        {
            var transaction = await _context.FinancialTransactions.FindAsync(transactionId);

            if (transaction == null)
            {
                throw new InvalidOperationException($"Transaction with ID {transactionId} not found.");
            }

            transaction.Status = status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while updating the transaction status.", ex);
            }
        }

        // Delete (soft delete) a transaction
        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.FinancialTransactions.FindAsync(id);

            if (transaction == null)
            {
                throw new InvalidOperationException($"Transaction with ID {id} not found.");
            }

            try
            {
                transaction.Status = TransactionStatus.Cancelled;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the transaction.", ex);
            }
        }

        // Validate transaction
        private void ValidateTransaction(FinancialTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
            }

            if (string.IsNullOrEmpty(transaction.PayCurrency) || string.IsNullOrEmpty(transaction.ReceiveCurrency))
            {
                throw new InvalidOperationException("Currency details are required.");
            }

            if (transaction.AmountPaid <= 0 || transaction.AmountReceived <= 0)
            {
                throw new InvalidOperationException("Transaction amounts must be greater than zero.");
            }

            if (transaction.ConversionRate <= 0)
            {
                throw new InvalidOperationException("Conversion rate must be greater than zero.");
            }
        }
    }
}
