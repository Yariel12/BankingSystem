using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, List<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCustomersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();

        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            FullName = c.GetFullName(),
            Email = c.Email,
            Phone = c.Phone,
            IdentificationNumber = c.IdentificationNumber,
            DateOfBirth = c.DateOfBirth,
            CreatedAt = c.CreatedAt
        }).ToList();
    }
}