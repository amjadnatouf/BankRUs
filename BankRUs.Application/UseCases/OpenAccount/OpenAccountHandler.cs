using BankRUs.Application.Identity;
using BankRUs.Application.Repositories;
using BankRUs.Application.Services;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenAccount;

public class OpenAccountHandler
{
    private readonly IIdentityService _identityService;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IEmailService _emailService;

    public OpenAccountHandler(
        IIdentityService identityService,
        IBankAccountRepository bankAccountRepository,
        IEmailService emailService)
    {
        _identityService = identityService;
        _bankAccountRepository = bankAccountRepository;
        _emailService = emailService;
    }

    public async Task<OpenAccountResult> HandleAsync(OpenAccountCommand command)
    {
        // 1. Skapa användarkonto (ASP.NET Core Identity)
        var createUserResult = await _identityService.CreateUserAsync(new CreateUserRequest(
            FirstName: command.FirstName,
            LastName: command.LastName,
            SocialSecurityNumber: command.SocialSecurityNumber,
            Email: command.Email
         ));

        // 2. Skapa bankkonto
        var bankAccount = new BankAccount
        {
            Id = Guid.NewGuid(),
            AccountNumber = GenerateAccountNumber(),
            Balance = 0,
            UserId = createUserResult.UserId.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        await _bankAccountRepository.CreateAsync(bankAccount);

        // 3. Skicka välkomstmail till kund
        await _emailService.SendWelcomeEmailAsync(
            command.Email,
            command.FirstName,
            bankAccount.AccountNumber);

        return new OpenAccountResult(UserId: createUserResult.UserId);
    }

    private static string GenerateAccountNumber()
    {
        // Generera ett 10-siffrigt kontonummer
        var random = new Random();
        var accountNumber = string.Empty;

        for (int i = 0; i < 10; i++)
        {
            accountNumber += random.Next(0, 10).ToString();
        }

        return accountNumber;
    }
}