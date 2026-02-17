using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Persistance;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // BankAccount konfiguration
        builder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            // Unique index on AccountNumber
            entity.HasIndex(e => e.AccountNumber)
                .IsUnique()
                .HasDatabaseName("IX_BankAccounts_AccountNumber");

            // Relationship with Transactions
            entity.HasMany<Transaction>()
                .WithOne(t => t.BankAccount)
                .HasForeignKey(t => t.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Transaction konfiguration
        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.BankAccountId).IsRequired();
            entity.Property(e => e.Type).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.Reference).HasMaxLength(140);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.BalanceAfter).IsRequired().HasColumnType("decimal(18,2)");

            // Index for queries
            entity.HasIndex(e => new { e.BankAccountId, e.CreatedAt })
                .HasDatabaseName("IX_Transactions_BankAccountId_CreatedAt");
        });

        // ApplicationUser konfiguration - lägg till unika index
        builder.Entity<ApplicationUser>(entity =>
        {
            // Unikt index på SocialSecurityNumber
            entity.HasIndex(e => e.SocialSecurityNumber)
                .IsUnique()
                .HasDatabaseName("IX_AspNetUsers_SocialSecurityNumber");

            // Email är redan unikt via NormalizedEmail index från Identity
            entity.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("IX_AspNetUsers_Email")
                .HasFilter("[Email] IS NOT NULL");
        });
    }
}