using Microsoft.AspNetCore.Identity;

namespace VaaradhiPay.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? UserRefId { get; set; }
        public string? ProfilePicturePath { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public ICollection<BankAccount> BankAccounts { get; set; }
        public ICollection<UPIAddress> UPIAddresses { get; set; }
        public ICollection<TetherWallet> TetherWallets { get; set; }
        public ICollection<KYCDetails> KYCDetails { get; set; }

    }

}
