using BankSystem.Application.Commands;
using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

namespace BankSystem.Application.Handlers;

public class DepositMoneyHandler : IRequestHandler<DepositMoneyCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public DepositMoneyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(
        DepositMoneyCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);

        if (account == null)
            throw new InvalidOperationException("La cuenta no existe");

        if (account.CustomerId != request.RequestingCustomerId)
            throw new UnauthorizedAccessException("No tienes permiso para realizar esta operación");

        account.Deposit(request.Amount, request.Description);

        await _unitOfWork.SaveChangesAsync();

        var transaction = account.Transactions
            .OrderByDescending(t => t.CreatedAt)
            .First();

        return new TransactionDto
        {
            Id = transaction.Id,
            AccountId = account.Id,
            AccountNumber = account.AccountNumber.Value,
            TransactionType = transaction.TransactionType.ToString(),
            Amount = transaction.Amount.Amount,
            Currency = transaction.Amount.Currency,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate
        };
    }
}
