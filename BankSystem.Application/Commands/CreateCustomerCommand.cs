using BankSystem.Application.DTOs;
using MediatR;

namespace BankSystem.Application.Commands
{
    public class CreateCustomerCommand : IRequest<CustomerDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string IdentificationNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
