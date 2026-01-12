using BankSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }


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


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
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


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
    {
        try
        {
            var customer = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear el cliente", error = ex.Message });
        }
    }


    [HttpGet("{id}/accounts")]
    public async Task<IActionResult> GetAccounts(Guid id)
    {
        try
        {
            var query = new GetAccountsByCustomerIdQuery { CustomerId = id };
            var accounts = await _mediator.Send(query);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener las cuentas", error = ex.Message });
        }
    }
}