using BankSystem.Application.Commands;
using BankSystem.Application.DTOs;
using BankSystem.Domain.Interfaces;
using MediatR;

namespace BankSystem.Application.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginHandler(
        ICustomerRepository customerRepository,
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByEmailAsync(request.Email);

        if (customer == null)
            throw new UnauthorizedAccessException("Email o contraseña incorrectos");

        if (!customer.IsActive)
            throw new UnauthorizedAccessException("La cuenta está desactivada");

        if (!customer.VerifyPassword(request.Password))
            throw new UnauthorizedAccessException("Email o contraseña incorrectos");

        customer.RecordLogin();
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtTokenService.GenerateToken(
            customer.Id,
            customer.Email,
            customer.GetFullName());

        return new LoginResponse(
            token,
            customer.Email,
            customer.GetFullName(),
            customer.Id);
    }
}