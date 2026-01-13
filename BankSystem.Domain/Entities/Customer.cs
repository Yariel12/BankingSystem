using BankSystem.Domain.Common;
using System.Security.Cryptography;
using System.Text;

namespace BankSystem.Domain.Entities;

public class Customer : BaseEntity
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string IdentificationNumber { get; private set; } = null!;
    public DateTime DateOfBirth { get; private set; }

    public string PasswordHash { get; private set; } = null!;
    public string PasswordSalt { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginAt { get; private set; }

    private readonly List<Account> _accounts = new();
    public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

    private Customer() { }

    public Customer(
        string firstName,
        string lastName,
        string email,
        string phone,
        string identificationNumber,
        DateTime dateOfBirth,
        string password)
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
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres", nameof(password));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        IdentificationNumber = identificationNumber;
        DateOfBirth = dateOfBirth;

        SetPassword(password);
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres");

        PasswordSalt = GenerateSalt();
        PasswordHash = HashPassword(password, PasswordSalt);
        SetUpdatedAt();
    }

    public bool VerifyPassword(string password)
    {
        var hash = HashPassword(password, PasswordSalt);
        return hash == PasswordHash;
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        SetUpdatedAt();
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

    public void UpdateContactInfo(string email, string phone)
    {
        if (!string.IsNullOrWhiteSpace(email))
            Email = email;
        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone;
        SetUpdatedAt();
    }

    public string GetFullName() => $"{FirstName} {LastName}";

    private static string GenerateSalt()
    {
        var saltBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    private static string HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hash);
    }
}