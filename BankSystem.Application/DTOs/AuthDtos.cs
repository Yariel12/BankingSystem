namespace BankSystem.Application.DTOs;

public record LoginRequest(string Email, string Password);

public record LoginResponse(
    string Token,
    string Email,
    string FullName,
    Guid CustomerId);

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string IdentificationNumber,
    DateTime DateOfBirth,
    string Password);