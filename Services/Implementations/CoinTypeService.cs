using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class CoinTypeService : ICoinTypeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaginationService<CoinType> _paginationService;

        public CoinTypeService(ApplicationDbContext context, IPaginationService<CoinType> paginationService)
        {
            _context = context;
            _paginationService = paginationService;
        }

        public async Task<List<CoinType>> GetPaginatedCoinTypesAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.CoinTypes
                .Where(c => !c.IsDeleted && c.IsActive &&
                            (string.IsNullOrEmpty(searchTerm) || c.Name.ToLower().Contains(searchTerm.ToLower())))
                .OrderBy(c => c.Name);

            var paginatedQuery = _paginationService.ApplyPagination(query, page, pageSize);
            return await Task.FromResult(paginatedQuery.ToList());
        }

        public async Task<List<CoinType>> GetActiveCoinsAsync()
        {
            return await _context.CoinTypes
                .Where(c => c.IsActive && !c.IsDeleted)
                .ToListAsync();
        }


        public async Task<CoinType> GetCoinTypeByIdAsync(int id)
        {
            return await _context.CoinTypes.FindAsync(id);
        }

        public async Task AddCoinTypeAsync(CoinType coinType)
        {
            _context.CoinTypes.Add(coinType);
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task UpdateCoinTypeAsync(CoinType coinType)
        {
            var existing = await _context.CoinTypes.FindAsync(coinType.CoinTypeId);
            if (existing == null) return;

            existing.Name = coinType.Name;
            existing.Symbol = coinType.Symbol;
            existing.IsActive = coinType.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCoinTypeAsync(int id)
        {
            var coinType = await _context.CoinTypes.FindAsync(id);
            if (coinType != null)
            {
                coinType.IsDeleted = true; // Soft delete
                coinType.IsActive = false; // Deactivate the coin
                await _context.SaveChangesAsync();
            }
        }

        public async Task ToggleCoinActiveStatusAsync(int id)
        {
            var coinType = await _context.CoinTypes.FindAsync(id);
            if (coinType == null) return;

            coinType.IsActive = !coinType.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task BulkAddCoinTypesAsync(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
                throw new FileNotFoundException("JSON file not found", jsonFilePath);

            var jsonData = await File.ReadAllTextAsync(jsonFilePath);
            var coinTypes = JsonSerializer.Deserialize<List<CoinType>>(jsonData);

            if (coinTypes == null || !coinTypes.Any())
                throw new InvalidOperationException("No data found in the JSON file");

            // Fetch all existing symbols from the database
            var existingSymbols = await _context.CoinTypes.Select(c => c.Symbol).ToListAsync();

            // Filter out duplicates
            var newCoinTypes = coinTypes.Where(c => !existingSymbols.Contains(c.Symbol)).ToList();

            if (newCoinTypes.Any())
            {
                _context.CoinTypes.AddRange(newCoinTypes);
                await _context.SaveChangesAsync();
            }
        }

    }
}
