using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Interfaces
{
     public interface IUPIAddressService
    {
        Task<List<UPIAddress>> GetPaginatedUPIAddressesAsync(string userId, string searchTerm, int page, int pageSize);
        Task<UPIAddress?> GetUPIAddressByIdAsync(int id);
        Task<List<UPIAddress>> GetActiveUPIAddressesByUserAsync(string userId);
        Task AddUPIAddressAsync(UPIAddress upiAddress);
        Task UpdateUPIAddressAsync(UPIAddress upiAddress);
        Task DeleteUPIAddressAsync(int id);
    }
}
