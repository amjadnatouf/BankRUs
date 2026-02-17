using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.Deposit;

public class DepositHandler
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public DepositHandler(
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<DepositResult> HandleAsync(DepositCommand command)
    {
        // 1. Hämta bankkonto
        var bankAccount = await _bankAccountRepository.GetByIdAsync(command.BankAccountId);

        if (bankAccount is null)
        {
            throw new InvalidOperationException($"Bank account with ID {command.BankAccountId} not found");
        }

        // 2. Uppdatera saldo
        bankAccount.Balance += command.Amount;

        // 3. Skapa transaktion
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            BankAccountId = command.BankAccountId,
            Type = "deposit",
            Amount = command.Amount,
            Currency = "SEK",
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            BalanceAfter = bankAccount.Balance
        };

        await _transactionRepository.CreateAsync(transaction);

        // 4. Spara uppdaterat bankkonto
        await _bankAccountRepository.UpdateAsync(bankAccount);

        return new DepositResult(
            TransactionId: transaction.Id,
            UserId: Guid.Parse(bankAccount.UserId),
            Type: transaction.Type,
            Amount: transaction.Amount,
            Currency: transaction.Currency,
            Reference: transaction.Reference,
            CreatedAt: transaction.CreatedAt,
            BalanceAfter: transaction.BalanceAfter
        );
    }
}