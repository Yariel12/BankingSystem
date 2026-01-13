using BankSystem.Application.DTOs;
using MediatR;

namespace BankSystem.Application.Commands;

public record RegisterCustomerAuthCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string IdentificationNumber,
    DateTime DateOfBirth,
    string Password) : IRequest<CustomerDto>;