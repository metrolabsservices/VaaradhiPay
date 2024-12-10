using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;
using VaaradhiPay.Services.Interfaces;

namespace VaaradhiPay.Services.Implementations
{
    public class LoggedInUserService : ILoggedInUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoggedInUserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<LoggedInUserDTO?> GetLoggedInUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return null; // User is not logged in
            }

            // Extract UserId from claims
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return null; // User ID not found in claims
            }

            // Fetch user details from the database
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null; // User not found
            }

            // Map user details to DTO
            return new LoggedInUserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserRefId = user.UserRefId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public string? GetLoggedInUserName()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return null; // User is not logged in
            }

            // Extract UserName from claims
            return httpContext.User.Identity?.Name;
        }

        public string? GetUserClaim(string claimType)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return null; // User is not logged in
            }

            // Extract the specified claim from the claims collection
            return httpContext.User.FindFirstValue(claimType);
        }
    }
}
