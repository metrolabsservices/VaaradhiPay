using System.Threading.Tasks;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Interfaces
{
    public interface ILoggedInUserService
    {
        /// <summary>
        /// Gets the details of the currently logged-in user.
        /// </summary>
        Task<LoggedInUserDTO?> GetLoggedInUserAsync();

        /// <summary>
        /// Gets the username of the currently logged-in user.
        /// </summary>
        string? GetLoggedInUserName();

        /// <summary>
        /// Gets a specific claim value for the logged-in user.
        /// </summary>
        string? GetUserClaim(string claimType);
    }
}
