using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VaaradhiPay.Data;

namespace VaaradhiPay.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<List<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string roleName);

    }
}
