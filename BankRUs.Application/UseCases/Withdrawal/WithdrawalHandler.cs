using BankRUs.Application.Repositories;
using BankRUs.Domain.Entities;

namespace BankRUs.Application.UseCases.Withdrawal;

public class WithdrawalHandler
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public WithdrawalHandler(
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<WithdrawalResult> HandleAsync(WithdrawalCommand command)
    {
        // 1. Hämta bankkonto
        var bankAccount = await _bankAccountRepository.GetByIdAsync(command.BankAccountId);

        if (bankAccount is null)
        {
            throw new InvalidOperationException($"Bank account with ID {command.BankAccountId} not found");
        }

        // 2. Kontrollera att saldo räcker
        if (bankAccount.Balance < command.Amount)
        {
            throw new InsufficientFundsException(
                $"Account balance is {bankAccount.Balance:F2} SEK but withdrawal amount is {command.Amount:F2} SEK.",
                bankAccount.Balance,
                command.Amount);
        }

        // 3. Uppdatera saldo
        bankAccount.Balance -= command.Amount;

        // 4. Skapa transaktion
        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            BankAccountId = command.BankAccountId,
            Type = "withdrawal",
            Amount = command.Amount,
            Currency = "SEK",
            Reference = command.Reference,
            CreatedAt = DateTime.UtcNow,
            BalanceAfter = bankAccount.Balance
        };

        await _transactionRepository.CreateAsync(transaction);

        // 5. Spara uppdaterat bankkonto
        await _bankAccountRepository.UpdateAsync(bankAccount);

        return new WithdrawalResult(
            TransactionId: transaction.Id,
            AccountId: command.BankAccountId,
            Type: transaction.Type,
            Amount: transaction.Amount,
            Currency: transaction.Currency,
            Reference: transaction.Reference,
            CreatedAt: transaction.CreatedAt,
            BalanceAfter: transaction.BalanceAfter
        );
    }
}

// Custom exception for insufficient funds
public class InsufficientFundsException : Exception
{
    public decimal CurrentBalance { get; }
    public decimal RequestedAmount { get; }

    public InsufficientFundsException(string message, decimal currentBalance, decimal requestedAmount)
        : base(message)
    {
        CurrentBalance = currentBalance;
        RequestedAmount = requestedAmount;
    }
}