namespace BankSystem.Domain.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Guid customerId, string email, string fullName);
    Guid? ValidateToken(string token);
}