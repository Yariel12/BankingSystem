using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankSystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Solo puedes ver transacciones de TUS cuentas
    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetByAccountId(Guid accountId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
            var accountQuery = new GetAccountByIdQuery { AccountId = accountId };
            var account = await _mediator.Send(accountQuery);

            if (account == null)
                return NotFound(new { message = "Cuenta no encontrada" });

            var customerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (account.CustomerId != customerId)
                return Forbid();

            var query = new GetTransactionHistoryQuery
            {
                AccountId = accountId,
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