using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

public class GetAccountByNumberHandler : IRequestHandler<GetAccountByNumberQuery, AccountDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccountByNumberHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountDto?> Handle(GetAccountByNumberQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByAccountNumberAsync(request.AccountNumber);
        if (account == null) return null;

        var customer = await _unitOfWork.Customers.GetByIdAsync(account.CustomerId);

        return new AccountDto
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber.Value,
            Balance = account.Balance.Amount,
            Currency = account.Balance.Currency,
            AccountType = account.AccountType.ToString(),
            IsActive = account.IsActive,
            CustomerId = account.CustomerId,
            CustomerName = customer?.GetFullName() ?? "N/A",
            CreatedAt = account.CreatedAt
        };
    }
}