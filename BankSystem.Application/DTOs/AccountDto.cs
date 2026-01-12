namespace BankSystem.Application.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = null!;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public bool IsActive { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
