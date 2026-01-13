using BankSystem.Application.DTOs;
using MediatR;

namespace BankSystem.Application.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;