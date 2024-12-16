using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VaaradhiPay.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Define roles
            string[] roles = { "User", "Admin", "SuperAdmin" };

            // Seed roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Define default accounts
            var accounts = new[]
            {
            new { Email = "user1@domain.com", Password = "User@123", Role = "User", FirstName = "User", LastName = "One", KYCstatus = "Not Started" },
            new { Email = "user2@domain.com", Password = "User@123", Role = "User", FirstName = "User", LastName = "Two", KYCstatus = "Not Started" },
            new { Email = "user3@domain.com", Password = "User@123", Role = "User", FirstName = "User", LastName = "Three", KYCstatus = "Not Started" },
            new { Email = "admin1@domain.com", Password = "Admin@123", Role = "Admin", FirstName = "Admin", LastName = "One", KYCstatus = "Approved" },
            new { Email = "admin2@domain.com", Password = "Admin@123", Role = "Admin", FirstName = "Admin", LastName = "Two", KYCstatus = "Approved" },
            new { Email = "superadmin1@domain.com", Password = "SuperAdmin@123", Role = "SuperAdmin", FirstName = "SuperAdmin", LastName = "One", KYCstatus = "Approved" },
            new { Email = "superadmin2@domain.com", Password = "SuperAdmin@123", Role = "SuperAdmin", FirstName = "SuperAdmin", LastName = "Two", KYCstatus = "Approved" }
        };

            // Seed accounts
            foreach (var account in accounts)
            {
                var user = await userManager.FindByEmailAsync(account.Email);
                if (user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = account.Email,
                        Email = account.Email,
                        EmailConfirmed = true,
                        FirstName = account.FirstName,
                        LastName = account.LastName,
                        KYCstatus = account.KYCstatus // Include KYCstatus
                    };

                    var createResult = await userManager.CreateAsync(newUser, account.Password);
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, account.Role);
                        Console.WriteLine($"Account created: {account.Email} with role {account.Role}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create account: {account.Email}. Errors: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"Account already exists: {account.Email}");
                }
            }
        }
    }

}
