using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.OpenBankAccount;

public class OpenBankAccountHandler
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IUserRepository _userRepository;

    public OpenBankAccountHandler(
        IBankAccountRepository bankAccountRepository,
        IUserRepository userRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _userRepository = userRepository;
    }

    public async Task<OpenBankAccountResult> HandleAsync(OpenBankAccountCommand command)
    {
        // 1. Verifiera att användaren finns
        var userExists = await _userRepository.UserExistsAsync(command.UserId);

        if (!userExists)
        {
            throw new InvalidOperationException($"User with ID {command.UserId} not found");
        }

        // 2. Skapa nytt bankkonto
        var bankAccount = new BankAccount
        {
            Id = Guid.NewGuid(),
            AccountNumber = GenerateAccountNumber(),
            Balance = 0,
            UserId = command.UserId.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        await _bankAccountRepository.CreateAsync(bankAccount);

        return new OpenBankAccountResult(
            BankAccountId: bankAccount.Id,
            AccountNumber: bankAccount.AccountNumber);
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