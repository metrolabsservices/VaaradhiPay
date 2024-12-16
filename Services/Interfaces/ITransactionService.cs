using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services
{
    public interface ITransactionService
    {
        Task<List<FinancialTransaction>> GetPaginatedTransactionsAsync(
            string searchTerm,
            int page,
            int pageSize,
            DateTime? startDate = null,
            DateTime? endDate = null,
            TransactionStatus? status = null);

        Task<FinancialTransaction> GetTransactionByIdAsync(int id);

        Task AddTransactionAsync(FinancialTransaction transaction);

        Task UpdateTransactionAsync(FinancialTransaction transaction);

        Task UpdateTransactionStatusAsync(int transactionId, TransactionStatus status);

        Task DeleteTransactionAsync(int id);
    }
}
