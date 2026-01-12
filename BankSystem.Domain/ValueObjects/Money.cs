namespace BankSystem.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;

    private Money() { }

    public Money(decimal amount, string currency = "DOP")
    {
        if (amount < 0)
            throw new ArgumentException("El monto no puede ser negativo", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("La moneda es requerida", nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("No se pueden sumar montos de diferentes monedas");

        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("No se pueden restar montos de diferentes monedas");

        if (Amount < other.Amount)
            throw new InvalidOperationException("Fondos insuficientes");

        return new Money(Amount - other.Amount, Currency);
    }

    public static bool operator >(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("No se pueden comparar montos de diferentes monedas");

        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("No se pueden comparar montos de diferentes monedas");

        return left.Amount < right.Amount;
    }

    public static bool operator >=(Money left, Money right)
        => left > right || left.Amount == right.Amount;

    public static bool operator <=(Money left, Money right)
        => left < right || left.Amount == right.Amount;

    public override bool Equals(object? obj)
        => obj is Money other && Amount == other.Amount && Currency == other.Currency;

    public override int GetHashCode()
        => HashCode.Combine(Amount, Currency);

    public override string ToString()
        => $"{Amount:N2} {Currency}";
}
