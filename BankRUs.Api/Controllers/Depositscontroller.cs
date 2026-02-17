using BankRUs.Api.Dtos.Transactions;
using BankRUs.Api.Dtos.Transactions.Deposit;
using BankRUs.Application.UseCases.Deposit;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

[Route("api/bank-accounts/{bankAccountId}/deposits")]
[ApiController]
public class DepositsController : ControllerBase
{
    private readonly DepositHandler _depositHandler;

    public DepositsController(DepositHandler depositHandler)
    {
        _depositHandler = depositHandler;
    }

    // POST /api/bank-accounts/{bankAccountId}/deposits
    [HttpPost]
    public async Task<IActionResult> Create(Guid bankAccountId, DepositRequestDto request)
    {
        try
        {
            var result = await _depositHandler.HandleAsync(
                new DepositCommand(
                    BankAccountId: bankAccountId,
                    Amount: request.Amount,
                    Reference: request.Reference
                ));

            var response = new DepositResponseDto(
                TransactionId: result.TransactionId,
                UserId: result.UserId,
                Type: result.Type,
                Amount: result.Amount,
                Currency: result.Currency,
                Reference: result.Reference,
                CreatedAt: result.CreatedAt,
                BalanceAfter: result.BalanceAfter
            );

            return Created(string.Empty, response);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }
}