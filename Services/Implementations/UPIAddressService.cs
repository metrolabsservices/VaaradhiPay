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
    public class UPIAddressService : IUPIAddressService
    {
        private readonly ApplicationDbContext _context;

        public UPIAddressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UPIAddress>> GetPaginatedUPIAddressesAsync(string userId, string searchTerm, int page, int pageSize)
        {
            var query = _context.UPIAddresses
                .Where(u => u.UserId == userId && (string.IsNullOrEmpty(searchTerm) || u.Address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .OrderByDescending(u => u.CreatedDate);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<UPIAddress?> GetUPIAddressByIdAsync(int id)
        {
            return await _context.UPIAddresses
                .FirstOrDefaultAsync(u => u.UPIAddressId == id);
        }

        public async Task<List<UPIAddress>> GetActiveUPIAddressesByUserAsync(string userId)
        {
            return await _context.UPIAddresses
                .Where(u => u.UserId == userId && u.IsActive)
                .OrderBy(u => u.CreatedDate)
                .ToListAsync();
        }

        public async Task AddUPIAddressAsync(UPIAddress upiAddress)
        {
            if (upiAddress == null) throw new ArgumentNullException(nameof(upiAddress));

            await _context.UPIAddresses.AddAsync(upiAddress);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUPIAddressAsync(UPIAddress upiAddress)
        {
            var existingAddress = await _context.UPIAddresses.FindAsync(upiAddress.UPIAddressId);
            if (existingAddress == null) return;

            existingAddress.Address = upiAddress.Address;
            existingAddress.UpiUserName = upiAddress.UpiUserName;
            existingAddress.IsActive = upiAddress.IsActive;
            existingAddress.CreatedDate = upiAddress.CreatedDate; // Optional, if allowing date modification

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUPIAddressAsync(int id)
        {
            var upiAddress = await _context.UPIAddresses.FindAsync(id);
            if (upiAddress == null) return;

            upiAddress.IsActive = false; // Soft delete
            await _context.SaveChangesAsync();
        }

   

    }
}
