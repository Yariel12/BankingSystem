using BankSystem.Application.DTOs;
using MediatR;

public class DepositMoneyCommand : IRequest<TransactionDto>
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = "Depósito";
}