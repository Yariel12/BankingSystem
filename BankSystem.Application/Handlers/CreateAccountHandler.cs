using BankSystem.Application.Commands;
using BankSystem.Application.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Enums;
using BankSystem.Domain.Interfaces;
using MediatR;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
        if (customer == null)
            throw new InvalidOperationException("El cliente no existe");

        var account = new Account(
            request.CustomerId,
            (AccountType)request.AccountType,
            request.InitialDeposit,
            request.Currency);

        await _unitOfWork.Accounts.AddAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return new AccountDto
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber.Value,
            Balance = account.Balance.Amount,
            Currency = account.Balance.Currency,
            AccountType = account.AccountType.ToString(),
            IsActive = account.IsActive,
            CustomerId = account.CustomerId,
            CustomerName = customer.GetFullName(),
            CreatedAt = account.CreatedAt
        };
    }
}