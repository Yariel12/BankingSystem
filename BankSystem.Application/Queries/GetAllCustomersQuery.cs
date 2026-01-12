using BankSystem.Application.DTOs;
using MediatR;

public class GetAllCustomersQuery : IRequest<List<CustomerDto>>
{
}