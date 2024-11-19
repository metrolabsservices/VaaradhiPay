using System.Collections.Generic;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services
{
    public interface IUserService
    {
        Task<(List<ApplicationUser> Users, int TotalCount)> GetUsersAsync(string searchTerm, int page, int pageSize);
        Task<List<ApplicationUser>> GetUsersWithDetailsAsync(string searchTerm, int page, int pageSize);

        Task<Dictionary<string, string>> GetUserStatusesAsync(string userId);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, string roleName);
        Task<bool> RemoveRoleAsync(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
    }
}
