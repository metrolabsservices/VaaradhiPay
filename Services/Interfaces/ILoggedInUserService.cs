using System.Threading.Tasks;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Interfaces
{
    public interface ILoggedInUserService
    {
        /// Gets the details of the currently logged-in user.
        Task<LoggedInUserDTO?> GetLoggedInUserAsync();

        /// Gets the username of the currently logged-in user.
        string? GetLoggedInUserName();

        /// Gets a specific claim value for the logged-in user.
        string? GetUserClaim(string claimType);

        /// Gets the username of the currently logged-in user ID.
        string? GetLoggedInUserId();
    }
}
