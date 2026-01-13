using BankSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Solo admins deberían ver TODOS los customers (por ahora lo dejamos protegido)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllCustomersQuery();
            var customers = await _mediator.Send(query);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los clientes", error = ex.Message });
        }
    }

    // Solo puedes ver tu propio perfil o cualquier customer (validación por ID)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var authenticatedCustomerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (id != authenticatedCustomerId)
                return Forbid(); // 403 - No tienes permiso

            var query = new GetCustomerByIdQuery { CustomerId = id };
            var customer = await _mediator.Send(query);

            if (customer == null)
                return NotFound(new { message = "Cliente no encontrado" });

            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el cliente", error = ex.Message });
        }
    }

    // Solo puedes ver TUS cuentas
    [HttpGet("{id}/accounts")]
    public async Task<IActionResult> GetAccounts(Guid id)
    {
        try
        {
            var authenticatedCustomerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (id != authenticatedCustomerId)
                return Forbid(); // 403 - No puedes ver cuentas de otros

            var query = new GetAccountsByCustomerIdQuery { CustomerId = id };
            var accounts = await _mediator.Send(query);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener las cuentas", error = ex.Message });
        }
    }

    // Ver MI perfil (más práctico)
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        try
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var query = new GetCustomerByIdQuery { CustomerId = customerId };
            var customer = await _mediator.Send(query);

            if (customer == null)
                return NotFound(new { message = "Cliente no encontrado" });

            return Ok(customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener el perfil", error = ex.Message });
        }
    }

    // Ver MIS cuentas (más práctico)
    [HttpGet("me/accounts")]
    public async Task<IActionResult> GetMyAccounts()
    {
        try
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var query = new GetAccountsByCustomerIdQuery { CustomerId = customerId };
            var accounts = await _mediator.Send(query);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener las cuentas", error = ex.Message });
        }
    }
}