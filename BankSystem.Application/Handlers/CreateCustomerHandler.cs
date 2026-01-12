using BankSystem.Application.Commands;
using BankSystem.Application.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Interfaces;
using MediatR;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var existingByEmail = await _unitOfWork.Customers.GetByEmailAsync(request.Email);
        if (existingByEmail != null)
            throw new InvalidOperationException("Ya existe un cliente con ese email");

        var existingByIdentification = await _unitOfWork.Customers.GetByIdentificationNumberAsync(request.IdentificationNumber);
        if (existingByIdentification != null)
            throw new InvalidOperationException("Ya existe un cliente con esa cédula");

        var customer = new Customer(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.IdentificationNumber,
            request.DateOfBirth);

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            FullName = customer.GetFullName(),
            Email = customer.Email,
            Phone = customer.Phone,
            IdentificationNumber = customer.IdentificationNumber,
            DateOfBirth = customer.DateOfBirth,
            CreatedAt = customer.CreatedAt
        };
    }
}
