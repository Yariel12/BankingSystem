using BankSystem.Application.DTOs;
using MediatR;

public class GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    public Guid CustomerId { get; set; }
}