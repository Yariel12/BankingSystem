using BankSystem.Application.DTOs;
using MediatR;

namespace BankSystem.Application.Commands;

public class DepositMoneyCommand : IRequest<TransactionDto>
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid RequestingCustomerId { get; set; } // ← NUEVO
}