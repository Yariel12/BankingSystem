using BankSystem.Application.DTOs;
using MediatR;

public class GetAccountsByCustomerIdQuery : IRequest<List<AccountDto>>
{
    public Guid CustomerId { get; set; }
}