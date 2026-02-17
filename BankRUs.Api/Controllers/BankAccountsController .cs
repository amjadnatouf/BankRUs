using BankRUs.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

[Route("api/bank-accounts")]
[ApiController]
public class BankAccountsController : ControllerBase
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public BankAccountsController(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    // GET /api/bank-accounts?accountNumber=1234567890
    [HttpGet]
    public async Task<IActionResult> GetByAccountNumber([FromQuery] string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return BadRequest(new { error = "Account number is required" });
        }

        var bankAccount = await _bankAccountRepository.GetByAccountNumberAsync(accountNumber);

        if (bankAccount == null)
        {
            return NotFound(new { error = "Bank account not found" });
        }

        return Ok(new
        {
            id = bankAccount.Id,
            accountNumber = bankAccount.AccountNumber,
            balance = bankAccount.Balance,
            userId = bankAccount.UserId,
            createdAt = bankAccount.CreatedAt
        });
    }

    // GET /api/bank-accounts/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var bankAccount = await _bankAccountRepository.GetByIdAsync(id);

        if (bankAccount == null)
        {
            return NotFound(new { error = "Bank account not found" });
        }

        return Ok(new
        {
            id = bankAccount.Id,
            accountNumber = bankAccount.AccountNumber,
            balance = bankAccount.Balance,
            userId = bankAccount.UserId,
            createdAt = bankAccount.CreatedAt
        });
    }
}