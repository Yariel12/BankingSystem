
using BankSystem.Application.DTOs;
using MediatR;

namespace BankSystem.Application.Commands
{
    public class CreateAccountCommand : IRequest<AccountDto>
    {
        public Guid CustomerId { get; set; }
        public int AccountType { get; set; } 
        public decimal InitialDeposit { get; set; }
        public string Currency { get; set; } = "DOP";
    }
}
