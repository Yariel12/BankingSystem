using BankSystem.Application.DTOs;
using MediatR;

public class GetAccountByNumberQuery : IRequest<AccountDto?>
{
    public string AccountNumber { get; set; } = string.Empty;
}