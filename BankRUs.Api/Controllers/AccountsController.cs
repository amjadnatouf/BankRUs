using BankRUs.Api.Dtos.Accounts;
using BankRUs.Application.UseCases.OpenAccount;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly OpenAccountHandler _openAccountHandler;

    public AccountsController(OpenAccountHandler openAccountHandler)
    {
        _openAccountHandler = openAccountHandler;
    }

    // POST /api/accounts
    [HttpPost]
    public async Task<IActionResult> Create(CreateAccountRequestDto request)
    {
        try
        {
            var openAccountResult = await _openAccountHandler.HandleAsync(
                new OpenAccountCommand(
                    FirstName: request.FirstName,
                    LastName: request.LastName,
                    SocialSecurityNumber: request.SocialSecurityNumber,
                    Email: request.Email));

            var response = new CreateAccountResponseDto(openAccountResult.UserId);

            // Returnera 201 Created
            return Created(string.Empty, response);
        }
        catch (InvalidOperationException ex) when (
            ex.Message.Contains("social security number") ||
            ex.Message.Contains("email"))
        {
            // 409 Conflict för dubbletter
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // 500 Internal Server Error för andra fel
            //return StatusCode(500, new { error = "An error occurred while creating the account" });
            return StatusCode(500, new
            {
                error = ex.Message,
                detail = ex.InnerException?.Message,
                stack = ex.StackTrace
            });
        }
    }
}
