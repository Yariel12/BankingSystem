using BankSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Solo puedes ver cuentas que te pertenecen
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var query = new GetAccountByIdQuery { AccountId = id };
            var account = await _mediator.Send(query);

            if (account == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            // Validar que la cuenta pertenece al customer autenticado
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (account.CustomerId != customerId)
                return Forbid();

            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener la cuenta", error = ex.Message });
        }
    }

    // Buscar por número de cuenta (útil para transferencias)
    [HttpGet("number/{accountNumber}")]
    public async Task<IActionResult> GetByNumber(string accountNumber)
    {
        try
        {
            var query = new GetAccountByNumberQuery { AccountNumber = accountNumber };
            var account = await _mediator.Send(query);

            if (account == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (account.CustomerId != customerId)
                return Forbid();

            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener la cuenta", error = ex.Message });
        }
    }

    // Crear cuenta - debe ser para el customer autenticado
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
    {
        try
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            if (command.CustomerId != customerId)
                return Forbid();

            var account = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear la cuenta", error = ex.Message });
        }
    }

    // ✅ DEPOSITAR - Optimizado (sin doble carga)
    [HttpPost("{id}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] DepositRequest request)
    {
        try
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var command = new DepositMoneyCommand
            {
                AccountId = id,
                Amount = request.Amount,
                Description = request.Description ?? "Depósito",
                RequestingCustomerId = customerId // ← Ahora el handler valida esto
            };

            var transaction = await _mediator.Send(command);
            return Ok(transaction);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al realizar el depósito", error = ex.Message });
        }
    }

    // ✅ RETIRAR - Optimizado (sin doble carga)
    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] WithdrawRequest request)
    {
        try
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var command = new WithdrawMoneyCommand
            {
                AccountId = id,
                Amount = request.Amount,
                Description = request.Description ?? "Retiro",
                RequestingCustomerId = customerId // ← Ahora el handler valida esto
            };

            var transaction = await _mediator.Send(command);
            return Ok(transaction);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al realizar el retiro", error = ex.Message });
        }
    }

    // ✅ TRANSFERIR - Optimizado (sin doble carga)
    [HttpPost("{id}/transfer")]
    public async Task<IActionResult> Transfer(Guid id, [FromBody] TransferRequest request)
    {
        try
        {
            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var command = new TransferMoneyCommand
            {
                SourceAccountId = id,
                DestinationAccountNumber = request.DestinationAccountNumber,
                Amount = request.Amount,
                Description = request.Description ?? "Transferencia",
                RequestingCustomerId = customerId // ← Ahora el handler valida esto
            };

            var transaction = await _mediator.Send(command);
            return Ok(transaction);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al realizar la transferencia", error = ex.Message });
        }
    }

    // Ver transacciones - solo de TUS cuentas
    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var query = new GetAccountByIdQuery { AccountId = id };
            var account = await _mediator.Send(query);

            if (account == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (account.CustomerId != customerId)
                return Forbid();

            var transactionsQuery = new GetTransactionHistoryQuery
            {
                AccountId = id,
                StartDate = startDate,
                EndDate = endDate
            };

            var transactions = await _mediator.Send(transactionsQuery);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener las transacciones", error = ex.Message });
        }
    }
}

public record DepositRequest(decimal Amount, string? Description);
public record WithdrawRequest(decimal Amount, string? Description);
public record TransferRequest(string DestinationAccountNumber, decimal Amount, string? Description);