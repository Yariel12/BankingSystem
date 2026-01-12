namespace BankSystem.Domain.ValueObjects;

public class AccountNumber
{
    public string Value { get; private set; } = null!;

    private AccountNumber() { }

    public AccountNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El número de cuenta es requerido", nameof(value));

        if (value.Length != 10)
            throw new ArgumentException("El número de cuenta debe tener 10 dígitos", nameof(value));

        if (!value.All(char.IsDigit))
            throw new ArgumentException("El número de cuenta solo puede contener dígitos", nameof(value));

        Value = value;
    }

    public static AccountNumber Generate()
    {
        var random = new Random();
        var accountNumber = string.Empty;

        for (int i = 0; i < 10; i++)
            accountNumber += random.Next(0, 10);

        return new AccountNumber(accountNumber);
    }

    public override bool Equals(object? obj)
        => obj is AccountNumber other && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value;

    public static implicit operator string(AccountNumber accountNumber)
        => accountNumber.Value;
}
