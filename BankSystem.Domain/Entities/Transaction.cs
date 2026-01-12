using BankSystem.Domain.Common;
using BankSystem.Domain.Enums;
using BankSystem.Domain.ValueObjects;

namespace BankSystem.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid AccountId { get; private set; }
    public Account Account { get; private set; } = null!;

    public TransactionType TransactionType { get; private set; }
    public Money Amount { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime TransactionDate { get; private set; }

    public Guid? RelatedAccountId { get; private set; }

    private Transaction() { }

    public Transaction(
        Guid accountId,
        TransactionType transactionType,
        Money amount,
        string description,
        Guid? relatedAccountId = null)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción es requerida", nameof(description));

        AccountId = accountId;
        TransactionType = transactionType;
        Amount = amount;
        Description = description;
        TransactionDate = DateTime.UtcNow;
        RelatedAccountId = relatedAccountId;
    }
}
