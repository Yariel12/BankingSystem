using BankSystem.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var query = new GetAccountByIdQuery { AccountId = id };
            var account = await _mediator.Send(query);

            if (account == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener la cuenta", error = ex.Message });
        }
    }


    [HttpGet("number/{accountNumber}")]
    public async Task<IActionResult> GetByNumber(string accountNumber)
    {
        try
        {
            var query = new GetAccountByNumberQuery { AccountNumber = accountNumber };
            var account = await _mediator.Send(query);

            if (account == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener la cuenta", error = ex.Message });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
    {
        try
        {
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


    [HttpPost("{id}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] DepositRequest request)
    {
        try
        {
            var command = new DepositMoneyCommand
            {
                AccountId = id,
                Amount = request.Amount,
                Description = request.Description ?? "Depósito"
            };

            var transaction = await _mediator.Send(command);
            return Ok(transaction);
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


    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] WithdrawRequest request)
    {
        try
        {
            var command = new WithdrawMoneyCommand
            {
                AccountId = id,
                Amount = request.Amount,
                Description = request.Description ?? "Retiro"
            };

            var transaction = await _mediator.Send(command);
            return Ok(transaction);
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


    [HttpPost("{id}/transfer")]
    public async Task<IActionResult> Transfer(Guid id, [FromBody] TransferRequest request)
    {
        try
        {
            var command = new TransferMoneyCommand
            {
                SourceAccountId = id,
                DestinationAccountNumber = request.DestinationAccountNumber,
                Amount = request.Amount,
                Description = request.Description ?? "Transferencia"
            };

            var transaction = await _mediator.Send(command);
            return Ok(transaction);
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


    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetTransactions(Guid id, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var query = new GetTransactionHistoryQuery
            {
                AccountId = id,
                StartDate = startDate,
                EndDate = endDate
            };

            var transactions = await _mediator.Send(query);
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