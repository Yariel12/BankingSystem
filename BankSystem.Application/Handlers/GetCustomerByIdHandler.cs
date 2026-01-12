using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetWithAccountsAsync(request.CustomerId);
        if (customer == null) return null;

        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            FullName = customer.GetFullName(),
            Email = customer.Email,
            Phone = customer.Phone,
            IdentificationNumber = customer.IdentificationNumber,
            DateOfBirth = customer.DateOfBirth,
            CreatedAt = customer.CreatedAt,
            Accounts = customer.Accounts.Select(a => new AccountDto
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber.Value,
                Balance = a.Balance.Amount,
                Currency = a.Balance.Currency,
                AccountType = a.AccountType.ToString(),
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt
            }).ToList()
        };
    }
}