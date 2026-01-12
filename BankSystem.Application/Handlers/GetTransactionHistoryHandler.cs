using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

public class GetTransactionHistoryHandler : IRequestHandler<GetTransactionHistoryQuery, List<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTransactionHistoryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TransactionDto>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
    {
        var transactions = request.StartDate.HasValue && request.EndDate.HasValue
            ? await _unitOfWork.Transactions.GetByDateRangeAsync(request.AccountId, request.StartDate.Value, request.EndDate.Value)
            : await _unitOfWork.Transactions.GetByAccountIdAsync(request.AccountId);

        var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            AccountId = t.AccountId,
            AccountNumber = account?.AccountNumber.Value ?? "N/A",
            TransactionType = t.TransactionType.ToString(),
            Amount = t.Amount.Amount,
            Currency = t.Amount.Currency,
            Description = t.Description,
            TransactionDate = t.TransactionDate,
            RelatedAccountId = t.RelatedAccountId
        }).ToList();
    }
}