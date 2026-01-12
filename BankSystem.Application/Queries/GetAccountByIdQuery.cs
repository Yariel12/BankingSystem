using BankSystem.Application.DTOs;
using MediatR;

public class GetAccountByIdQuery : IRequest<AccountDto?>
{
    public Guid AccountId { get; set; }
}