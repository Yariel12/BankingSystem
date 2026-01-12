using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetByAccountId(Guid accountId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        try
        {
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