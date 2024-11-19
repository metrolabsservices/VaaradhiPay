using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Fetch paginated users with optional search
       public async Task<List<ApplicationUser>> GetUsersAsync(string searchTerm, int page, int pageSize)
{
    var query = _context.Users
        .Include(u => u.KYCDetails)
        .Include(u => u.BankAccounts)
        .Include(u => u.TetherWallets)
        .Include(u => u.UPIAddresses)
        .AsQueryable();

    if (!string.IsNullOrEmpty(searchTerm))
    {
        query = query.Where(u =>
            u.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            u.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    return await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}

        public async Task<List<ApplicationUser>> GetUsersWithDetailsAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.Users
                .Include(u => u.KYCDetails)
                .Include(u => u.BankAccounts)
                .Include(u => u.TetherWallets)
                .Include(u => u.UPIAddresses)
                .AsQueryable(); // Ensure the query is IQueryable<ApplicationUser>

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        // Fetch user-specific status information
        public async Task<Dictionary<string, string>> GetUserStatusesAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.KYCDetails)
                .Include(u => u.BankAccounts)
                .Include(u => u.TetherWallets)
                .Include(u => u.UPIAddresses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;

            return new Dictionary<string, string>
            {
                { "KYC", user.KYCDetails?.Any(k => k.IsVerified) == true ? "Completed" : "Pending" },
                { "Bank", user.BankAccounts?.Any(b => b.IsActive) == true ? "Available" : "None" },
                { "Tether", user.TetherWallets?.Any(w => w.IsActive) == true ? "Available" : "None" },
                { "UPI", user.UPIAddresses?.Any(u => u.IsActive) == true ? "Available" : "None" }
            };
        }

        // Update user details
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null) return false;

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);
            return result.Succeeded;
        }

        // Delete user
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        // Assign a role to a user
        public async Task<bool> AssignRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(roleName)) return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        // Remove a role from a user
        public async Task<bool> RemoveRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || !await _roleManager.RoleExistsAsync(roleName)) return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        // Get roles assigned to a user
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null ? new List<string>(await _userManager.GetRolesAsync(user)) : null;
        }

        Task<(List<ApplicationUser> Users, int TotalCount)> IUserService.GetUsersAsync(string searchTerm, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
