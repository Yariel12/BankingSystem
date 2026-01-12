using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

public class GetAccountsByCustomerIdHandler : IRequestHandler<GetAccountsByCustomerIdQuery, List<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccountsByCustomerIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<AccountDto>> Handle(GetAccountsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Accounts.GetAccountsByCustomerIdAsync(request.CustomerId);
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);

        return accounts.Select(a => new AccountDto
        {
            Id = a.Id,
            AccountNumber = a.AccountNumber.Value,
            Balance = a.Balance.Amount,
            Currency = a.Balance.Currency,
            AccountType = a.AccountType.ToString(),
            IsActive = a.IsActive,
            CustomerId = a.CustomerId,
            CustomerName = customer?.GetFullName() ?? "N/A",
            CreatedAt = a.CreatedAt
        }).ToList();
    }
}