using BankSystem.Domain.Common;

namespace BankSystem.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string IdentificationNumber { get; private set; } = null!;
    public DateTime DateOfBirth { get; private set; }

    private readonly List<Account> _accounts = new();
    public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

    private Customer() { }

    public Customer(
        string firstName,
        string lastName,
        string email,
        string phone,
        string identificationNumber,
        DateTime dateOfBirth)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("El nombre es requerido", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido es requerido", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email es requerido", nameof(email));

        if (string.IsNullOrWhiteSpace(identificationNumber))
            throw new ArgumentException("La cédula es requerida", nameof(identificationNumber));

        if (dateOfBirth >= DateTime.UtcNow.AddYears(-18))
            throw new ArgumentException("El cliente debe ser mayor de 18 años");

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        IdentificationNumber = identificationNumber;
        DateOfBirth = dateOfBirth;
    }

    public void UpdateContactInfo(string email, string phone)
    {
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;

        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone;

        SetUpdatedAt();
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}
