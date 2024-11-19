using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VaaradhiPay.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace VaaradhiPay.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CoinType> CoinTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<UPIAddress> UPIAddresses { get; set; }
        public DbSet<TetherWallet> TetherWallets { get; set; }
        public DbSet<KYCDetails> KYCDetails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the relationship between CoinType and Transaction
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CoinType)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CoinTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            // Define relationships between ApplicationUser and other entities
            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.User)
                .WithMany(u => u.BankAccounts)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<UPIAddress>()
                .HasOne(u => u.User)
                .WithMany(a => a.UPIAddresses)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<TetherWallet>()
                .HasOne(w => w.User)
                .WithMany(u => u.TetherWallets)
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<KYCDetails>()
                .HasOne(k => k.User)
                .WithMany(u => u.KYCDetails)
                .HasForeignKey(k => k.UserId);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "3", Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                new IdentityRole { Id = "4", Name = "Dev", NormalizedName = "DEV" }
            );

        }
    }
}