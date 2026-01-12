using BankSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.AccountId)
            .IsRequired();

        builder.Property(t => t.TransactionType)
            .IsRequired()
            .HasConversion<int>();

        // Configuración del Value Object Money (Amount)
        builder.OwnsOne(t => t.Amount, m =>
        {
            m.Property(x => x.Amount)
                .HasColumnName("Amount")
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            m.Property(x => x.Currency)
                .HasColumnName("Currency")
                .IsRequired()
                .HasMaxLength(3)
                .HasDefaultValue("DOP");
        });

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(t => t.TransactionDate)
            .IsRequired();

        builder.Property(t => t.RelatedAccountId)
            .IsRequired(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Relación con Account
        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice para mejorar búsquedas por fecha
        builder.HasIndex(t => t.TransactionDate);

        // Query Filter para soft delete
        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}