using BankSystem.Domain.Common;
using BankSystem.Domain.Enums;
using BankSystem.Domain.ValueObjects;

namespace BankSystem.Domain.Entities;

public class Account : BaseEntity
{
    public AccountNumber AccountNumber { get; private set; } = null!;
    public Money Balance { get; private set; } = null!;
    public AccountType AccountType { get; private set; }
    public bool IsActive { get; private set; }

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    private Account() { }

    public Account(
        Guid customerId,
        AccountType accountType,
        decimal initialDeposit = 0,
        string currency = "DOP")
    {
        if (initialDeposit < 0)
            throw new ArgumentException("El depósito inicial no puede ser negativo");

        CustomerId = customerId;
        AccountNumber = AccountNumber.Generate();
        Balance = new Money(initialDeposit, currency);
        AccountType = accountType;
        IsActive = true;
    }

    public void Deposit(decimal amount, string description = "Depósito")
    {
        if (!IsActive)
            throw new InvalidOperationException("La cuenta está inactiva");

        if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero");

        var money = new Money(amount, Balance.Currency);
        Balance = Balance.Add(money);

        var transaction = new Transaction(
            Id,
            TransactionType.Deposit,
            money,
            description);

        _transactions.Add(transaction);
        SetUpdatedAt();
    }

    public void Withdraw(decimal amount, string description = "Retiro")
    {
        if (!IsActive)
            throw new InvalidOperationException("La cuenta está inactiva");

        if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero");

        var money = new Money(amount, Balance.Currency);

        if (Balance < money)
            throw new InvalidOperationException("Fondos insuficientes");

        Balance = Balance.Subtract(money);

        var transaction = new Transaction(
            Id,
            TransactionType.Withdrawal,
            money,
            description);

        _transactions.Add(transaction);
        SetUpdatedAt();
    }

    public void TransferTo(Account destinationAccount, decimal amount, string description = "Transferencia")
    {
        if (!IsActive)
            throw new InvalidOperationException("La cuenta origen está inactiva");

        if (!destinationAccount.IsActive)
            throw new InvalidOperationException("La cuenta destino está inactiva");

        if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero");

        if (Balance.Currency != destinationAccount.Balance.Currency)
            throw new InvalidOperationException("Las cuentas deben tener la misma moneda");

        var money = new Money(amount, Balance.Currency);

        if (Balance < money)
            throw new InvalidOperationException("Fondos insuficientes");

        // Retirar de la cuenta origen
        Balance = Balance.Subtract(money);
        var outgoingTransaction = new Transaction(
            Id,
            TransactionType.Transfer,
            money,
            $"{description} - Hacia {destinationAccount.AccountNumber}",
            destinationAccount.Id);
        _transactions.Add(outgoingTransaction);

        // Depositar en la cuenta destino
        destinationAccount.Balance = destinationAccount.Balance.Add(money);
        var incomingTransaction = new Transaction(
            destinationAccount.Id,
            TransactionType.Transfer,
            money,
            $"{description} - Desde {AccountNumber}",
            Id);
        destinationAccount._transactions.Add(incomingTransaction);

        SetUpdatedAt();
        destinationAccount.SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }
}