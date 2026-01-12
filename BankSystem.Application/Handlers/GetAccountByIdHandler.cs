using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccountByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
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