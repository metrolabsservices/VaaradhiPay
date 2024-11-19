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

        public async Task<List<Transaction>> GetPaginatedTransactionsAsync(
                                                 string searchTerm,
                                                 int page,
                                                 int pageSize,
                                                 DateTime? startDate = null,
                                                 DateTime? endDate = null,
                                                 string transactionType = "All")
        {
            // Validate pagination parameters
            if (page < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page and pageSize must be greater than zero.");
            }

            // Validate date range
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be later than end date.");
            }

            // Validate transaction type
            if (!string.IsNullOrEmpty(transactionType) &&
                transactionType != "All" &&
                !transactionType.Equals("Buy", StringComparison.OrdinalIgnoreCase) &&
                !transactionType.Equals("Sell", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Transaction type must be 'All', 'Buy', or 'Sell'.");
            }

            try
            {
                // Build the base query
                var query = _context.Transactions
                    .Include(t => t.CoinType) // Include CoinType for navigation properties
                    .Where(t => !t.IsDeleted); // Exclude soft-deleted transactions

                // Apply search term filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(t => t.ReferenceNumber != null &&
                                             t.ReferenceNumber.ToLower().Contains(searchTerm.ToLower()));
                }

                // Apply date range filters
                if (startDate.HasValue)
                {
                    query = query.Where(t => t.TransactionDateTime >= startDate.Value);
                }
                if (endDate.HasValue)
                {
                    query = query.Where(t => t.TransactionDateTime <= endDate.Value);
                }

                // Apply transaction type filter
                if (!string.IsNullOrEmpty(transactionType) && transactionType != "All")
                {
                    query = query.Where(t => t.TransactionType.Equals(transactionType, StringComparison.OrdinalIgnoreCase));
                }

                // Order by transaction date
                query = query.OrderBy(t => t.TransactionDateTime);

                // Apply pagination
                var transactions = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                return transactions;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while fetching transactions.", ex);
            }
        }



        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Transaction ID must be greater than zero.");
            }

            var transaction = await _context.Transactions
                .Include(t => t.CoinType)
                .FirstOrDefaultAsync(t => t.TransactionId == id && !t.IsDeleted);

            if (transaction == null)
            {
                throw new InvalidOperationException($"Transaction with ID {id} not found or has been deleted.");
            }

            return transaction;
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            // Validate required fields
            ValidateTransaction(transaction);

            try
            {
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while saving the transaction.", ex);
            }
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            // Validate required fields
            ValidateTransaction(transaction);

            // Validate CoinType
            var coinType = await _context.CoinTypes.FirstOrDefaultAsync(c =>
                c.CoinTypeId == transaction.CoinTypeId &&
                !c.IsDeleted &&
                c.IsActive);

            if (coinType == null)
            {
                throw new InvalidOperationException("The specified CoinType is either inactive or does not exist.");
            }

            // Retrieve the existing transaction
            var existing = await _context.Transactions
                .Include(t => t.CoinType)
                .FirstOrDefaultAsync(t => t.TransactionId == transaction.TransactionId && !t.IsDeleted);

            if (existing == null)
            {
                throw new InvalidOperationException($"Transaction with ID {transaction.TransactionId} does not exist or has been deleted.");
            }

            try
            {
                // Update properties
                existing.ReferenceNumber = transaction.ReferenceNumber;
                existing.CoinTypeId = transaction.CoinTypeId;
                existing.Volume = transaction.Volume;
                existing.Amount = transaction.Amount;
                existing.TransactionDateTime = transaction.TransactionDateTime;
                existing.TransactionType = transaction.TransactionType;

                // Save changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("The transaction was modified by another user. Please try again.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while updating the transaction.", ex);
            }
        }

        public async Task DeleteTransactionAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Transaction ID must be greater than zero.");
            }

            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                throw new InvalidOperationException($"Transaction with ID {id} does not exist.");
            }

            try
            {
                transaction.IsDeleted = true; // Soft delete
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("An error occurred while deleting the transaction.", ex);
            }
        }

        private void ValidateTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
            }

            if (string.IsNullOrEmpty(transaction.TransactionType) ||
                (transaction.TransactionType != "Buy" && transaction.TransactionType != "Sell"))
            {
                throw new InvalidOperationException("TransactionType must be either 'Buy' or 'Sell'.");
            }

            if (transaction.Volume <= 0)
            {
                throw new InvalidOperationException("Volume must be greater than zero.");
            }

            if (transaction.Amount <= 0)
            {
                throw new InvalidOperationException("Amount must be greater than zero.");
            }
        }
    }
}
