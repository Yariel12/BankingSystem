namespace BankSystem.Application.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public Guid? RelatedAccountId { get; set; }
    public string? RelatedAccountNumber { get; set; }
}