using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetPaginatedTransactionsAsync(string searchTerm, int page, int pageSize, DateTime? startDate, DateTime? endDate, string transactionType);
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id);
    }
}
