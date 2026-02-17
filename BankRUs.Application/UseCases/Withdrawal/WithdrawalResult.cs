namespace BankRUs.Application.UseCases.Withdrawal;

public record WithdrawalResult(
    Guid TransactionId,
    Guid AccountId,
    string Type,
    decimal Amount,
    string Currency,
    string? Reference,
    DateTime CreatedAt,
    decimal BalanceAfter
);