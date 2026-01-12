using BankSystem.Domain.Entities;
using BankSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Infrastructure.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.AccountNumber, an =>
        {
            an.Property(x => x.Value)
                .HasColumnName("AccountNumber")
                .IsRequired()
                .HasMaxLength(10);

            an.HasIndex(x => x.Value)
                .IsUnique();
        });

        builder.OwnsOne(a => a.Balance, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("Balance")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            m.Property(x => x.Currency)
                .HasColumnName("Currency")
                .IsRequired()
                .HasMaxLength(3)
                .HasDefaultValue("DOP");
        });

        builder.Property(a => a.AccountType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.CustomerId)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relación con Customer
        builder.HasOne(a => a.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación con Transactions
        builder.HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Query Filter para soft delete
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}