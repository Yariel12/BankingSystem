using BankSystem.Application.Commands;
using BankSystem.Application.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.Domain.Interfaces;
using MediatR;

namespace BankSystem.Application.Handlers;

public class RegisterCustomerAuthHandler : IRequestHandler<RegisterCustomerAuthCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerAuthHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto> Handle(RegisterCustomerAuthCommand request, CancellationToken cancellationToken)
    {
        if (await _customerRepository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("El email ya está registrado");

        var existingCustomer = await _customerRepository.GetByIdentificationNumberAsync(request.IdentificationNumber);
        if (existingCustomer != null)
            throw new InvalidOperationException("La cédula ya está registrada");

        var customer = new Customer(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.IdentificationNumber,
            request.DateOfBirth,
            request.Password);

        await _customerRepository.AddAsync(customer);
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
            CreatedAt = customer.CreatedAt,
            Accounts = new List<AccountDto>()
        };
    }
}