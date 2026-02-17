using BankRUs.Api.Dtos.Transactions;
using BankRUs.Application.UseCases.GetTransactions;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

[Route("api/bank-accounts/{bankAccountId}/transactions")]
[ApiController]
public class TransactionsController : ControllerBase
{
    private readonly GetTransactionsHandler _getTransactionsHandler;

    public TransactionsController(GetTransactionsHandler getTransactionsHandler)
    {
        _getTransactionsHandler = getTransactionsHandler;
    }

    // GET /api/bank-accounts/{bankAccountId}/transactions
    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        Guid bankAccountId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] string? type = null,
        [FromQuery] string sort = "desc")
    {
        try
        {
            // Validate pageSize
            if (pageSize > 100)
            {
                pageSize = 100;
            }

            if (pageSize < 1)
            {
                pageSize = 20;
            }

            if (page < 1)
            {
                page = 1;
            }

            var result = await _getTransactionsHandler.HandleAsync(
                new GetTransactionsQuery(
                    BankAccountId: bankAccountId,
                    Page: page,
                    PageSize: pageSize,
                    From: from,
                    To: to,
                    Type: type,
                    Sort: sort
                ));

            var response = new GetTransactionsResponseDto(
                AccountId: result.AccountId,
                Currency: result.Currency,
                Balance: result.Balance,
                Paging: new PagingDto(
                    Page: result.Paging.Page,
                    PageSize: result.Paging.PageSize,
                    TotalCount: result.Paging.TotalCount,
                    TotalPages: result.Paging.TotalPages
                ),
                Items: result.Items.Select(i => new TransactionItemDto(
                    TransactionId: i.TransactionId,
                    Type: i.Type,
                    Amount: i.Amount,
                    Reference: i.Reference,
                    CreatedAt: i.CreatedAt,
                    BalanceAfter: i.BalanceAfter
                ))
            );

            return Ok(response);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }
}