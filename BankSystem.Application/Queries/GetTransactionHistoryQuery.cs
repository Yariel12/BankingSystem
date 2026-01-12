using BankSystem.Application.DTOs;
using MediatR;

public class GetTransactionHistoryQuery : IRequest<List<TransactionDto>>
{
    public Guid AccountId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}