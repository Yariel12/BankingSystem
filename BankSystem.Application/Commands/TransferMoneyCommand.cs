using BankSystem.Application.DTOs;
using MediatR;

public class TransferMoneyCommand : IRequest<TransactionDto>
{
    public Guid SourceAccountId { get; set; }
    public string DestinationAccountNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid RequestingCustomerId { get; set; }
}