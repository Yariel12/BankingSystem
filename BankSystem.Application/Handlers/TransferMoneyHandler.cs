using BankSystem.Application.DTOs;
using MediatR;
using BankSystem.Domain.Interfaces;

public class TransferMoneyHandler : IRequestHandler<TransferMoneyCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransferMoneyHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TransactionDto> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var sourceAccount = await _unitOfWork.Accounts.GetByIdAsync(request.SourceAccountId);
            if (sourceAccount == null)
                throw new InvalidOperationException("La cuenta origen no existe");

            var destinationAccount = await _unitOfWork.Accounts.GetByAccountNumberAsync(request.DestinationAccountNumber);
            if (destinationAccount == null)
                throw new InvalidOperationException("La cuenta destino no existe");

            sourceAccount.TransferTo(destinationAccount, request.Amount, request.Description);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

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
                TransactionDate = transaction.TransactionDate,
                RelatedAccountId = destinationAccount.Id,
                RelatedAccountNumber = destinationAccount.AccountNumber.Value
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}