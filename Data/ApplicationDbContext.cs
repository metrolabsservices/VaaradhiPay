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
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<UPIAddress> UPIAddresses { get; set; }
        public DbSet<TetherWallet> TetherWallets { get; set; }
        public DbSet<KYCDetails> KYCDetails { get; set; }
        public DbSet<Currency> Currencies { get; set; } 
        public DbSet<CurrencyExtractionAudit> CurrencyExtractionAudits { get; set; }
        public DbSet<AdminBankAccount> AdminBankAccounts { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships for TransactionRecord
            modelBuilder.Entity<FinancialTransaction>()
                 .HasOne(ft => ft.User)
                 .WithMany(u => u.TransactionRecords) // Ensure this matches the navigation property in ApplicationUser
                 .HasForeignKey(ft => ft.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FinancialTransaction>()
               .HasOne(ft => ft.AdminBankAccount)
               .WithMany(ab => ab.TransactionRecords) // Ensure this matches the navigation property in AdminBankAccount
               .HasForeignKey(ft => ft.AdminBankAccountId)
               .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes


            modelBuilder.Entity<FinancialTransaction>()
                .HasOne(ft => ft.UserBankAccount)
                .WithMany()
                .HasForeignKey(ft => ft.UserBankAccountId)
                .OnDelete(DeleteBehavior.Restrict);



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