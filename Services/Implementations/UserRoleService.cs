using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user) as List<string>;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            if (await _userManager.IsInRoleAsync(user, roleName))
                return IdentityResult.Failed(new IdentityError { Description = "User is already in the role." });

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (!await _userManager.IsInRoleAsync(user, roleName))
                return IdentityResult.Failed(new IdentityError { Description = "User is not in the role." });

            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
