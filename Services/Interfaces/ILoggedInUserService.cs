using System.Threading.Tasks;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Interfaces
{
    public interface ILoggedInUserService
    {
        Task<LoggedInUserDTO?> GetLoggedInUserAsync();
        string? GetLoggedInUserName();
        string? GetUserClaim(string claimType);
        string? GetLoggedInUserId();
        Task<(string? UserId, string? UserName, string? Role, string? Error)> GetLoggedInUserDetailsAsync();

    }
}
