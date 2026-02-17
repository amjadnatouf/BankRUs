using BankRUs.Api.Dtos.Transactions.Withdrawal;
using BankRUs.Application.UseCases.Withdrawal;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

[Route("api/bank-accounts/{bankAccountId}/withdrawals")]
[ApiController]
public class WithdrawalsController : ControllerBase
{
    private readonly WithdrawalHandler _withdrawalHandler;

    public WithdrawalsController(WithdrawalHandler withdrawalHandler)
    {
        _withdrawalHandler = withdrawalHandler;
    }

    // POST /api/bank-accounts/{bankAccountId}/withdrawals
    [HttpPost]
    public async Task<IActionResult> Create(Guid bankAccountId, WithdrawalRequestDto request)
    {
        try
        {
            var result = await _withdrawalHandler.HandleAsync(
                new WithdrawalCommand(
                    BankAccountId: bankAccountId,
                    Amount: request.Amount,
                    Reference: request.Reference
                ));

            var response = new WithdrawalResponseDto(
                TransactionId: result.TransactionId,
                AccountId: result.AccountId,
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
        catch (InsufficientFundsException ex)
        {
            // Return 409 Conflict with problem details
            return Conflict(new
            {
                type = "https://httpstatuses.com/409",
                title = "Insufficient funds",
                status = 409,
                detail = ex.Message,
                traceId = HttpContext.TraceIdentifier
            });
        }
    }
}