using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

public class TransferMoneyHandler : IRequestHandler<TransferMoneyCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransferMoneyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
        var sourceAccount = await _unitOfWork.Accounts.GetByIdAsync(request.SourceAccountId);

        if (sourceAccount == null)
            throw new InvalidOperationException("La cuenta origen no existe");

        if (sourceAccount.CustomerId != request.RequestingCustomerId)
            throw new UnauthorizedAccessException("No tienes permiso para realizar esta operación");

        var destinationAccount = await _unitOfWork.Accounts.GetByAccountNumberAsync(request.DestinationAccountNumber);

        if (destinationAccount == null)
            throw new InvalidOperationException("La cuenta destino no existe");

        sourceAccount.TransferTo(destinationAccount, request.Amount, request.Description);

        await _unitOfWork.SaveChangesAsync();

        var transaction = sourceAccount.Transactions.Last();

        return new TransactionDto
        {
            Id = transaction.Id,
            AccountId = sourceAccount.Id,
            AccountNumber = sourceAccount.AccountNumber.Value,
            TransactionType = transaction.TransactionType.ToString(),
            Amount = transaction.Amount.Amount,
            Currency = transaction.Amount.Currency,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate
        };
    }
}